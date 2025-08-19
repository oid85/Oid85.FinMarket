namespace Oid85.FinMarket.Domain.Models.Algo;

public class StatisticalArbitrageStrategy
{
    public Guid StrategyId { get; set; }
    
    public double StartMoney { get; set; }

    public double EndMoney { get; set; }

    public (string First, string Second) Ticker { get; set; } = (string.Empty, string.Empty);
    
    public string Timeframe { get; set; } = string.Empty;
    
    public string StrategyDescription { get; set; } = string.Empty;
    
    public string StrategyName { get; set; } = string.Empty;
    
    public DateOnly StartDate => DateOnly.FromDateTime(Candles.First.First().DateTime);
    
    public DateOnly EndDate => DateOnly.FromDateTime(Candles.First.Last().DateTime);
    
    public Dictionary<string, int> Parameters { get; set; } = new();
    
    public int StabilizationPeriod { get; set; }
    
    public (List<Candle> First, List<Candle> Second) Candles { get; set; } = (new(), new());
    
    public (List<double> First, List<double> Second) OpenPrices => (Candles.First.Select(x => x.Open).ToList(), Candles.Second.Select(x => x.Open).ToList());
    
    public (List<double> First, List<double> Second) ClosePrices => (Candles.First.Select(x => x.Close).ToList(), Candles.Second.Select(x => x.Close).ToList());
    
    public (List<double> First, List<double> Second) HighPrices => (Candles.First.Select(x => x.High).ToList(), Candles.Second.Select(x => x.High).ToList());
    
    public (List<double> First, List<double> Second) LowPrices => (Candles.First.Select(x => x.Low).ToList(), Candles.Second.Select(x => x.Low).ToList());

    public List<RegressionTailItem> Spreads { get; set; } = [];
    
    public List<ArbitrageGraphPoint> GraphPoints { get; set; } = [];
    
    public bool SignalLongShort { get; set; }
    
    public bool SignalShortLong { get; set; }

    public bool FilterLongShort { get; set; } = true;

    public bool FilterShortLong { get; set; } = true;
    
    public bool SignalCloseLongShort { get; set; }
    
    public bool SignalCloseShortLong { get; set; }
    
    public List<ArbitragePosition> Positions { get; set; } = new();
    
    public (int First, int Second) GetPositionSize((double First, double Second) orderPrice)
    {
        double money = EndMoney;
        
        if (money <= orderPrice.First + orderPrice.Second)
            return (0, 0);

        // По половине капитала для каждой ноги арбитража
        money /= 2.0;
        
        int positionSizeFirst = (int) Math.Round(money / orderPrice.First);
        int positionSizeSecond = (int) Math.Round(money / orderPrice.Second);

        return (positionSizeFirst, positionSizeSecond);
    }

    public ArbitragePosition? LastActivePosition {
        get
        {
            if (LastPosition is null)
                return null;
            
            if (!LastPosition.IsActive)
                return null;

            return LastPosition;
        }
    }

    public ArbitragePosition? LastPosition => Positions.Count == 0 ? null : Positions.Last();
    
    public (int First, int Second) CurrentPosition
    {
        get
        {
            if (LastActivePosition == null)
                return (0, 0);

            if (LastActivePosition.IsLongShort)
                return (Math.Abs(LastActivePosition.Quantity.First), -1 * Math.Abs(LastActivePosition.Quantity.Second));

            if (LastActivePosition.IsShortLong)
                return (-1 * Math.Abs(LastActivePosition.Quantity.First), Math.Abs(LastActivePosition.Quantity.Second));

            return (0, 0);
        }
    }

    public double CurrentPositionCost
    {
        get
        {
            if (LastActivePosition == null)
                return 0.0;

            if (LastActivePosition.IsLongShort)
                return LastActivePosition.Cost;

            if (LastActivePosition.IsShortLong)
                return -1 * Math.Abs(LastActivePosition.Cost);

            return 0.0;
        }
    }
    
    public void BuySellAtPrice((int First, int Second) quantity, (double First, double Second) price, int candleIndex) =>
        AddArbitrageTrade(new ArbitrageTrade
        {
            CandleIndex = candleIndex,
            Quantity = (Math.Abs(quantity.First), -1 * Math.Abs(quantity.Second)),
            Price = (price.First, price.Second),
            DateTime = Candles.First[candleIndex].DateTime
        });

    public void SellBuyAtPrice((int First, int Second) quantity, (double First, double Second) price, int candleIndex) =>
        AddArbitrageTrade(new ArbitrageTrade
        {
            CandleIndex = candleIndex,
            Quantity = (-1 * Math.Abs(quantity.First), Math.Abs(quantity.Second)),
            Price = (price.First, price.Second),
            DateTime = Candles.First[candleIndex].DateTime
        });

    private void AddArbitrageTrade(ArbitrageTrade trade)
    {
        if (trade.Quantity.First == 0 || trade.Quantity.Second == 0)
            return;
        
        if (LastActivePosition is null)
            Positions.Add(new ArbitragePosition
            {
                Ticker = (Ticker.First, Ticker.Second),
                EntryPrice = (trade.Price.First, trade.Price.Second),
                EntryDateTime = trade.DateTime,
                EntryCandleIndex = trade.CandleIndex,
                IsActive = true,
                IsLongShort = trade.Quantity is { First: > 0, Second: < 0 },
                IsShortLong = trade.Quantity is { First: < 0, Second: > 0 },
                Quantity = (trade.Quantity.First, trade.Quantity.Second),
                Cost = Math.Abs(trade.Quantity.First) * trade.Price.First + Math.Abs(trade.Quantity.Second) * trade.Price.Second
            });

        else
        {
            int count = Positions.Count;

            Positions[count - 1].ExitPrice = (trade.Price.First, trade.Price.Second);
            Positions[count - 1].ExitDateTime = trade.DateTime;
            Positions[count - 1].ExitCandleIndex = trade.CandleIndex;
            Positions[count - 1].IsActive = false;
                
            double profit = 0.0;

            if (Positions[count - 1].IsLongShort)
            {
                profit += Math.Abs(Positions[count - 1].Quantity.First) * (Positions[count - 1].ExitPrice.First - Positions[count - 1].EntryPrice.First);
                profit += Math.Abs(Positions[count - 1].Quantity.Second) * (Positions[count - 1].EntryPrice.Second - Positions[count - 1].ExitPrice.Second);
            }

            if (Positions[count - 1].IsShortLong)
            {
                profit += Math.Abs(Positions[count - 1].Quantity.First) * (Positions[count - 1].EntryPrice.First - Positions[count - 1].ExitPrice.First);
                profit += Math.Abs(Positions[count - 1].Quantity.Second) * (Positions[count - 1].ExitPrice.Second - Positions[count - 1].EntryPrice.Second);
            }
            
            Positions[count - 1].Profit = profit;
            Positions[count - 1].ProfitPercent = profit / EndMoney * 100.0;
            
            var totalProfit = Positions.Sum(x => x.Profit);
            Positions[count - 1].TotalProfit = totalProfit;
            Positions[count - 1].TotalProfitPct = totalProfit / EndMoney * 100.0;
            
            EndMoney += profit;
            
            EqiutyCurve.TryAdd(Positions[count - 1].ExitDateTime, Positions[count - 1].TotalProfit);
            
            double drawdown;
            
            if (count < 2)
                drawdown = 0.0;

            else
            {
                var maxTotalProfit = Positions.Take(count - 1).Max(x => x.TotalProfit);

                drawdown = Positions[count - 1].TotalProfit >= maxTotalProfit
                    ? 0.0
                    : maxTotalProfit - Positions[count - 1].TotalProfit;
            }
            
            DrawdownCurve.TryAdd(Positions[count - 1].ExitDateTime, drawdown);
        }
    }
    
    public void CloseAtPrice(ArbitragePosition position, (double First, double Second) price, int candleIndex)
    {
        // Отправляем команды, если длинная позиция
        if (position.IsLongShort)
            SellBuyAtPrice((position.Quantity.First, position.Quantity.Second), (price.First, price.Second), candleIndex);

        // Отправляем команды, если короткая позиция
        else if (position.IsShortLong)
            BuySellAtPrice((position.Quantity.First, position.Quantity.Second), (price.First, price.Second), candleIndex);
    }

    public Dictionary<DateTime, double> EqiutyCurve { get; set; } = new();
    
    public Dictionary<DateTime, double> DrawdownCurve  { get; set; } = new();
    
    public double ProfitFactor
    {
        get
        {
            double profits = Positions.Where(x => x.Profit > 0.0).Sum(x => x.Profit);
            double losses = Positions.Where(x => x.Profit < 0.0).Sum(x => x.Profit);

            if (losses == 0.0)
                return double.PositiveInfinity;
            
            return profits / Math.Abs(losses);
        }
    }

    public double RecoveryFactor => MaxDrawdown == 0.0 ? double.PositiveInfinity : NetProfit / MaxDrawdown;

    public double NetProfit => LastPosition?.TotalProfit ?? 0.0;
    
    public double AverageNetProfit => Positions.Count == 0 ? 0.0 : Positions.Select(x => x.Profit).Average();

    public double AverageNetProfitPercent => Positions.Count == 0 ? 0.0 : Positions.Select(x => x.ProfitPercent).Average();

    public double Drawdown  => LastPosition is null ? 0.0 : Positions.Max(x => x.TotalProfit) - LastPosition.TotalProfit;

    public double MaxDrawdown  => DrawdownCurve.Count == 0 ? 0.0 : Math.Abs(DrawdownCurve.Max(x => x.Value));

    public double MaxDrawdownPercent => EqiutyCurve.Count == 0 ? 0.0 : Math.Abs(MaxDrawdown / EqiutyCurve.Max(x => x.Value) * 100.0);

    public int NumberPositions => Positions.Count;

    public int WinningPositions => Positions.Count == 0 ? 0 : Positions.Count(x => x.Profit > 0.0);

    public double WinningTradesPercent => NumberPositions == 0.0 ? 0.0 : Convert.ToDouble(WinningPositions) / Convert.ToDouble(NumberPositions) * 100.0;    
    
    public double TotalReturn => EndMoney > StartMoney ? (EndMoney - StartMoney) / StartMoney * 100.0 : 0.0;
    
    public double AnnualYieldReturn => EndMoney > StartMoney ? TotalReturn / ((EndDate.DayNumber - StartDate.DayNumber) / 365.0): 0.0;
    
    public virtual void Execute()
    {

    }
}
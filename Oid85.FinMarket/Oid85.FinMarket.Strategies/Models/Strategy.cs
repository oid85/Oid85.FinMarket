namespace Oid85.FinMarket.Strategies.Models;

public class Strategy
{
    public Dictionary<string, int> Parameters { get; set; } = new();
    
    public int StabilizationPeriod { get; set; }
    
    public List<Candle> Candles { get; set; } = new();  
    
    public List<double> OpenPrices => Candles.Select(x => x.Open).ToList();
    
    public List<double> ClosePrices => Candles.Select(x => x.Close).ToList();
    
    public List<double> HighPrices => Candles.Select(x => x.High).ToList();
    
    public List<double> LowPrices => Candles.Select(x => x.Low).ToList();
    
    public bool SignalLong { get; set; }
    
    public bool SignalShort { get; set; }
    
    public List<StopLimit?> StopLimits { get; set; } = new();
    
    public List<Trade> Trades { get; set; } = new();
    
    public List<Position> Positions
    {
        get
        {
            var positions = new List<Position>();

            if (Trades.Count == 0)
                return positions;

            var groupedTrades = new List<List<Trade>>();
            int currentPosSize = 0;

            for (int i = 0; i < Trades.Count; i++)
            {
                // Разбиваем каждую сделку на атомарную (атомарная сделка - сделка объемом 1)
                var atomarTrades = GetAtomarTrades(Trades[i]);

                for (int j = 0; j < atomarTrades.Count; j++)
                {
                    if (currentPosSize == 0)
                        groupedTrades.Add(new List<Trade>());

                    groupedTrades.Last().Add(atomarTrades[j]);
                    currentPosSize = groupedTrades.Last().Select(t => t.Quantity).Sum();
                }
            }

            // Формируем позиции
            for (int i = 0; i < groupedTrades.Count; i++)
            {
                int entryCandleIndex = groupedTrades[i].First().CandleIndex;
                int exitCandleIndex = groupedTrades[i].Last().CandleIndex;

                bool isLong = groupedTrades[i].First().Quantity > 0;
                bool isShort = groupedTrades[i].First().Quantity < 0;

                bool isActive = groupedTrades[i].Select(x => x.Quantity).Sum() != 0;

                List<Trade> buyTrades = groupedTrades[i].Where(x => x.Quantity > 0).OrderBy(x => x.DateTime).ToList();
                List<Trade> sellTrades = groupedTrades[i].Where(x => x.Quantity < 0).OrderBy(x => x.DateTime).ToList();

                DateTime entryDateTime = DateTime.MinValue;
                DateTime exitDateTime = DateTime.MinValue;
                double entryPrice = 0.0;
                double exitPrice = 0.0;
                int quantity = 0;
                double profit = 0.0;
                double profitPct = 0.0;

                if (isLong)
                {
                    entryDateTime = buyTrades.Last().DateTime;
                    quantity = Math.Abs(buyTrades.Select(trade => trade.Quantity).Sum());
                    entryPrice = Math.Abs(buyTrades.Sum(trade => trade.Price * trade.Quantity)) / quantity;

                    if (isActive)
                    {
                        exitDateTime = DateTime.MinValue;
                        exitPrice = 0.0;
                        profit = 0.0;
                        profitPct = 0.0;
                    }
                    else
                    {
                        exitDateTime = sellTrades.Last().DateTime;
                        exitPrice = Math.Abs(sellTrades.Sum(trade => trade.Price * trade.Quantity)) / quantity;
                        profit = (exitPrice - entryPrice) * quantity;
                        profitPct = profit / quantity / entryPrice * 100.0;
                    }
                }
                else if (isShort)
                {
                    entryDateTime = sellTrades.Last().DateTime;
                    quantity = Math.Abs(sellTrades.Select(trade => trade.Quantity).Sum());
                    entryPrice = Math.Abs(sellTrades.Sum(trade => trade.Price * trade.Quantity)) / quantity;

                    if (isActive)
                    {
                        exitDateTime = DateTime.MinValue;
                        exitPrice = 0.0;
                        profit = 0.0;
                        profitPct = 0.0;
                    }
                    else
                    {
                        exitDateTime = buyTrades.Last().DateTime;
                        exitPrice = Math.Abs(buyTrades.Sum(trade => trade.Price * trade.Quantity)) / quantity;
                        profit = (entryPrice - exitPrice) * quantity;
                        profitPct = profit / quantity / exitPrice * 100.0;
                    }
                }

                positions.Add(new Position
                {
                    EntryCandleIndex = entryCandleIndex,
                    ExitCandleIndex = exitCandleIndex,
                    EntryPrice = entryPrice,
                    ExitPrice = exitPrice,
                    Profit = profit,
                    ProfitPct = profitPct,
                    EntryDateTime = entryDateTime,
                    ExitDateTime = exitDateTime,
                    IsActive = isActive,
                    IsLong = isLong,
                    IsShort = isShort,
                    Quantity = isLong ? quantity : -1 * quantity
                });
            }

            positions[0].TotalProfit = positions[0].Profit;
            positions[0].TotalProfitPct = positions[0].ProfitPct;

            // Расчет общей прибыли
            for (int i = 1; i < positions.Count; i++)
            {
                positions[i].TotalProfit = positions[i - 1].TotalProfit + positions[i].Profit;
                positions[i].TotalProfitPct = positions[i - 1].TotalProfitPct + positions[i].ProfitPct;
            }

            return positions;
        }
    }

    private static List<Trade> GetAtomarTrades(Trade trade)
    {
        var result = new List<Trade>();

        for (int i = 0; i < Math.Abs(trade.Quantity); i++)
        {
            var atomarTrade = new Trade
            {
                CandleIndex = trade.CandleIndex,
                Price = trade.Price,
                Quantity = trade.Quantity > 0 ? 1 : -1,
                DateTime = trade.DateTime
            };

            result.Add(atomarTrade);
        }

        return result;
    }
    
    public Position? LastActivePosition => Positions.Count == 0 ? null : Positions.Last().IsActive ? Positions.Last() : null;

    public int CurrentPosition
    {
        get
        {
            if (LastActivePosition == null)
                return 0;

            if (LastActivePosition.IsLong)
                return LastActivePosition.Quantity;

            if (LastActivePosition.IsShort)
                return -1 * Math.Abs(LastActivePosition.Quantity);

            return 0;
        }
    }
    
    public double CurrentProfit => Positions.Count == 0 ? 0 : Positions.Last().TotalProfit;

    public void BuyAtPrice(int quantity, double price, int currentCandleIndex) =>
        Trades.Add(new Trade
        {
            CandleIndex = currentCandleIndex,
            Quantity = Math.Abs(quantity),
            Price = price,
            DateTime = Candles[currentCandleIndex].DateTime
        });

    public void SellAtPrice(int quantity, double price, int currentCandleIndex) =>
        Trades.Add(new Trade
        {
            CandleIndex = currentCandleIndex,
            Quantity = -1 * Math.Abs(quantity),
            Price = price,
            DateTime = Candles[currentCandleIndex].DateTime
        });
    
    public void CloseAtStop(Position? position, double stopPrice, int currentCandleIndex)
    {
        // Если позиции нет, то обнуляем стоп
        if (position is null)
        {
            StopLimits[currentCandleIndex - 1] = null;
            StopLimits[currentCandleIndex] = null;
            
            return;	
        }

        // Если позиция есть, а стопа еще нет, то выставляем
        StopLimits[currentCandleIndex] ??= new StopLimit
        {
            Quantity = position.Quantity,
            StopPrice = stopPrice
        };

        // Если стоп сработал, то отправляем команды
        if (position.IsLong && Candles[currentCandleIndex].Close <= StopLimits[currentCandleIndex]!.StopPrice)
            SellAtPrice(StopLimits[currentCandleIndex]!.Quantity, StopLimits[currentCandleIndex]!.StopPrice, currentCandleIndex);

        // Если стоп сработал, то отправляем команды
        else if (position.IsShort && Candles[currentCandleIndex].Close >= StopLimits[currentCandleIndex]!.StopPrice)
            BuyAtPrice(StopLimits[currentCandleIndex]!.Quantity, StopLimits[currentCandleIndex]!.StopPrice, currentCandleIndex);
    }    
    
    public void CloseAtPrice(Position position, double price, int currentCandleIndex)
    {
        // Отправляем команды, если длинная позиция
        if (position.IsLong)
            SellAtPrice(position.Quantity, price, currentCandleIndex);

        // Отправляем команды, если короткая позиция
        else if (position.IsShort)
            BuyAtPrice(position.Quantity, price, currentCandleIndex);
    }    
    
    public List<Tuple<DateTime, double>> EqiutyCurve
    {
        get
        {
            if (Positions.Count == 0)
                return [];

            if (Positions.Count == 1 && Positions.First().IsActive)
                return [];
            
            if (Positions.Count == 1 && !Positions.First().IsActive)
                return [];

            var eqiutyCurve = new List<Tuple<DateTime, double>>();

            eqiutyCurve.Add(new Tuple<DateTime, double>(Positions[0].ExitDateTime, Positions[0].Profit));

            for (int i = 1; i < Positions.Count; i++)
                if (!Positions[i].IsActive)
                    eqiutyCurve.Add(new Tuple<DateTime, double>(Positions[i].ExitDateTime, Positions[i - 1].Profit + Positions[i].Profit));

            return eqiutyCurve;
        }
    }    
   
    public List<Tuple<DateTime, double>> DrawdownCurve
    {
        get
        {
            if (Positions.Count == 0)
                return [];

            if (Positions.Count == 1 && Positions.First().IsActive)
                return [];

            if (Positions.Count == 1 && !Positions.First().IsActive)
                return [];

            var drawdownCurve = new List<Tuple<DateTime, double>>();

            double currentDrawdown = Positions[0].Profit > 0.0
                ? 0.0
                : Positions[0].Profit;

            drawdownCurve.Add(new Tuple<DateTime, double>(Positions[0].ExitDateTime, currentDrawdown));

            for (int i = 1; i < Positions.Count; i++)
                if (!Positions[i].IsActive)
                {
                    currentDrawdown = Positions[i - 1].Profit + Positions[i].Profit > 0.0
                        ? 0.0
                        : Positions[i - 1].Profit + Positions[i].Profit;

                    drawdownCurve.Add(new Tuple<DateTime, double>(Positions[i].ExitDateTime, currentDrawdown));
                }

            return drawdownCurve;
        }
    }    
    
    public double ProfitFactor
    {
        get
        {
            double grossProfit = Positions.Where(x => x.Profit > 0.0).Sum(x => x.Profit);
            double grossLoss = Positions.Where(x => x.Profit < 0.0).Sum(x => x.Profit);

            return grossProfit / Math.Abs(grossLoss);
        }
    }

    public double RecoveryFactor => NetProfit / MaxDrawdown;

    public double NetProfit => Positions.Last().TotalProfit;

    public double AverageProfit => Positions.Select(x => x.Profit).Average();

    public double AveragePercent => Positions.Select(x => x.ProfitPct).Average();

    public double Drawdown => DrawdownCurve.Last().Item2;

    public double MaxDrawdown => DrawdownCurve.Select(d => d.Item2).Min();

    public double MaxDrawdownPercent => Math.Abs(MaxDrawdown) / Math.Abs(NetProfit) * 100.0;

    public int NumberPositions => Positions.Count;

    public int WinningPositions => Positions.Count(x => x.Profit > 0.0);

    public double WinningTradesPercent => (double) WinningPositions / (double) NumberPositions * 100.0;    
    
    public double StartMoney => EqiutyCurve.Count == 0 ? 0.0 : EqiutyCurve.First().Item2;

    public double EndMoney => EqiutyCurve.Count == 0 ? 0.0 : EqiutyCurve.Last().Item2;

    public virtual void Execute()
    {

    }
}
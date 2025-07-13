using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Common.MathExtensions;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Strategies
{
    public class VolatilityBreakoutClassicShort(
        IIndicatorFactory indicatorFactory) 
        : Strategy
    {
        public override void Execute()
        {
            // Получаем параметры
            int period = Parameters["Period"];
            double multiplier = Parameters["Multiplier"] / 10.0;
            
            // Фильтр
            var filterEma = indicatorFactory.Ema(Candles, period);
            
            // Построение каналов волатильности
            List<double> price = OpenPrices.Add(ClosePrices)!.DivConst(2.0);
            List<double> atr = indicatorFactory.Atr(Candles, period);
            List<double> highLevel = price.Add(atr.MultConst(multiplier))!; // up = price + atr * multiplier;
            List<double> lowLevel = price.Sub(atr.MultConst(multiplier))!; // up = price - atr * multiplier;

            highLevel = indicatorFactory.Highest(highLevel, period);
            lowLevel = indicatorFactory.Lowest(lowLevel, period);
            
            highLevel = highLevel.Shift(1);
            lowLevel = lowLevel.Shift(1);
            
            // Сглаживание
            int smoothPeriod = 5;
            highLevel = indicatorFactory.Sma(highLevel, smoothPeriod);
            lowLevel = indicatorFactory.Sma(lowLevel, smoothPeriod);
            
            // Переменные для обслуживания позиции
            double startTrailing = 0.0;   // Стоп, выставляемый при открытии позиции
            double currentTrailing = 0.0; // Величина текущего стопа
            
            for (int i = StabilizationPeriod; i < Candles.Count - 1; i++)
            {
                SignalShort = Candles[i].Close < lowLevel[i];
                FilterShort = Candles[i].Close < filterEma[i];
                
                double orderPrice = Candles[i].Close;
                
                // Расчет размера позиции
                int positionSize = GetPositionSize(orderPrice);
                
                if (LastActivePosition is null)
                {
                    if (SignalShort && FilterShort)
                    {
                        startTrailing = highLevel[i];
                        SellAtPrice(positionSize, orderPrice, i + 1);
                    }
                }
                
                else
                {
                    if (LastActivePosition.IsShort)
                    {
                        int entryBar = LastActivePosition.EntryCandleIndex;
                        currentTrailing = i == entryBar ? startTrailing : Math.Min(currentTrailing, highLevel[i]);
                        
                        if (Candles[i].Close >= currentTrailing)
                            BuyAtPrice(positionSize, Candles[i].Close, i + 1);
                    }
                }
            }
        }
    }
}

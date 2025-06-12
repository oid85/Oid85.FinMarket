using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Common.MathExtensions;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Strategies
{
    public class VolatilityBreakoutClassicLong(
        IIndicatorFactory indicatorFactory) 
        : Strategy
    {
        public override void Execute()
        {
            // Получаем параметры
            int period = Parameters["Period"];
            double multiplier = Parameters["Multiplier"] / 10.0;
            
            // Построение каналов волатильности
            List<double> price = OpenPrices.Add(ClosePrices)!.DivConst(2.0);
            List<double> atr = indicatorFactory.Atr(Candles, period);
            List<double> highLevel = price.Add(atr.MultConst(multiplier))!; // up = price + atr * multiplier;
            List<double> lowLevel = price.Div(atr.MultConst(multiplier))!; // up = price - atr * multiplier;

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
                SignalLong = Candles[i].Close > highLevel[i];

                double orderPrice = Candles[i].Close;
                
                // Расчет размера позиции
                int positionSize = GetPositionSize(orderPrice);
                
                if (LastActivePosition == null)
                {
                    if (SignalLong)
                    {
                        startTrailing = lowLevel[i];
                        BuyAtPrice(positionSize, orderPrice, i + 1);
                    }
                }
                
                else
                {
                    if (LastActivePosition.IsLong)
                    {
                        int entryBar = LastActivePosition.EntryCandleIndex;
                        currentTrailing = i == entryBar ? startTrailing : Math.Max(currentTrailing, lowLevel[i]);
                        CloseAtStop(LastActivePosition, currentTrailing, i + 1);
                    }
                }
            }
        }
    }
}

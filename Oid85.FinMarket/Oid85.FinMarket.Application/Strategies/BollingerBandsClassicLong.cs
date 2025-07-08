using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Common.MathExtensions;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Strategies
{
    public class BollingerBandsClassicLong(
        IIndicatorFactory indicatorFactory) 
        : Strategy
    {
        public override void Execute()
        {
            // Получаем параметры
            int periodHighEntry = Parameters["PeriodEntry"];
            int periodLowExit = Parameters["PeriodExit"];
            double stdDev = Parameters["StdDev"] / 10.0;

            // Фильтр
            var filterEma = indicatorFactory.Ema(Candles, periodHighEntry);
            
            // Построение каналов
            var bollingerBandsEntry = indicatorFactory.BollingerBands(Candles, periodHighEntry, stdDev);
            var bollingerBandsExit = indicatorFactory.BollingerBands(Candles, periodLowExit, stdDev);
            List<double> highLevelEntry = bollingerBandsEntry.UpperBand;
            List<double> lowLevelExit = bollingerBandsExit.LowerBand;

            // Сдвиг вправо на одну свечу
            highLevelEntry = highLevelEntry.Shift(1);
            lowLevelExit = lowLevelExit.Shift(1);
            
            // Переменные для обслуживания позиции
            double trailingStop = 0.0;

            for (int i = StabilizationPeriod; i < Candles.Count - 1; i++)
            {
                // Правило входа
                SignalLong = ClosePrices[i] > highLevelEntry[i];
                FilterLong = Candles[i].Close > filterEma[i];
                
                // Задаем цену для заявки
                double orderPrice = Candles[i].Close;

                // Расчет размера позиции
                int positionSize = GetPositionSize(orderPrice);
                
                if (LastActivePosition is null)
                {
                    if (SignalLong && FilterLong)
                        BuyAtPrice(positionSize, orderPrice, i + 1);
                }
                
                else
                {
                    int entryCandleIndex = LastActivePosition.EntryCandleIndex;

                    if (LastActivePosition.IsLong)
                    {
                        double startTrailingStop = lowLevelExit[entryCandleIndex];
                        double curTrailingStop = lowLevelExit[i];

                        trailingStop = i == entryCandleIndex ? startTrailingStop : Math.Max(trailingStop, curTrailingStop);
                        
                        if (Candles[i].Close <= trailingStop)
                            SellAtPrice(positionSize, Candles[i].Close, i + 1);
                    }
                }
            }
        }
    }
}

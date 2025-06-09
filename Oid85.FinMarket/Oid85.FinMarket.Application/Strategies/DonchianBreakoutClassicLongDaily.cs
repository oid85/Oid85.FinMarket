using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Common.MathExtensions;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Strategies
{
    public class DonchianBreakoutClassicLongDaily(IIndicatorFactory indicatorFactory) : Strategy
    {
        public override void Execute()
        {
            int positionSize = 1;
            
            // Определяем периоды каналов
            int periodHighEntry = Parameters["PeriodEntry"];
            int periodLowExit = Parameters["PeriodExit"];

            // Цены для построения канала
            List<double> priceForChannelHighEntry = HighPrices.Add(LowPrices)!.Add(ClosePrices)!.Add(ClosePrices)!.DivConst(4.0);
            List<double> priceForChannelLowExit = HighPrices.Add(LowPrices)!.Add(ClosePrices)!.Add(ClosePrices)!.DivConst(4.0);

            // Построение каналов
            List<double> highLevelEntry = indicatorFactory.Highest(priceForChannelHighEntry, periodHighEntry);
            List<double> lowLevelExit = indicatorFactory.Lowest(priceForChannelLowExit, periodLowExit);

            // Сглаживание
            int smoothPeriod = 5;
            highLevelEntry = indicatorFactory.Sma(highLevelEntry, smoothPeriod);
            lowLevelExit = indicatorFactory.Sma(lowLevelExit, smoothPeriod);

            // Сдвиг вправо на одну свечу
            highLevelEntry = highLevelEntry.Shift(1);
            lowLevelExit = lowLevelExit.Shift(1);

            // Переменные для обслуживания позиции
            double trailingStop = 0.0;

            for (int i = StabilizationPeriod; i < Candles.Count - 1; i++)
            {
                try
                {
                    // Правило входа
                    SignalLong = ClosePrices[i] > highLevelEntry[i];

                    // Задаем цену для заявки
                    double orderPrice = Candles[i].Close;

                    if (LastActivePosition == null)
                    {
                        if (SignalLong)
                            BuyAtPrice(positionSize, orderPrice, i + 1);
                    }
                
                    else
                    {
                        int entryCandleIndex = LastActivePosition.EntryCandleIndex;

                        if (LastActivePosition.IsLong)
                        {
                            double startTrailingStop = lowLevelExit[entryCandleIndex];
                            double curTrailingStop = lowLevelExit[i];

                            trailingStop = i == entryCandleIndex
                                ? startTrailingStop
                                : Math.Max(trailingStop, curTrailingStop);

                            // Выход по стопу
                            CloseAtStop(LastActivePosition, trailingStop, i + 1);
                        }
                    }
                }
                
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}

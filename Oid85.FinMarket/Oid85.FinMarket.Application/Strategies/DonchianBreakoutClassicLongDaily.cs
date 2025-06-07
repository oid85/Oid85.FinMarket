using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Models;
using Oid85.FinMarket.Common.MathExtensions;

namespace Oid85.FinMarket.Application.Strategies
{
    public class DonchianBreakoutClassicLongDaily(IIndicatorFactory indicatorFactory) : Strategy
    {
        public override void Execute()
        {
            int positionSize = 1;
            
            // Определяем периоды каналов
            int periodHighEntry = Parameters["PeriodEntry"];
            int periodLowEntry = Parameters["PeriodEntry"];
            int periodHighExit = Parameters["PeriodExit"];
            int periodLowExit = Parameters["PeriodExit"];

            // Цены для построения канала
            List<double> priceForChannelHighEntry = HighPrices.Add(LowPrices)!.Add(ClosePrices)!.Add(ClosePrices)!.DivConst(4.0);
            List<double> priceForChannelHighExit = HighPrices.Add(LowPrices)!.Add(ClosePrices)!.Add(ClosePrices)!.DivConst(4.0);
            List<double> priceForChannelLowEntry = HighPrices.Add(LowPrices)!.Add(ClosePrices)!.Add(ClosePrices)!.DivConst(4.0);
            List<double> priceForChannelLowExit = HighPrices.Add(LowPrices)!.Add(ClosePrices)!.Add(ClosePrices)!.DivConst(4.0);

            // Построение каналов
            List<double> highLevelEntry = indicatorFactory.Highest(priceForChannelHighEntry, periodHighEntry);
            List<double> lowLevelEntry = indicatorFactory.Lowest(priceForChannelHighExit, periodLowEntry);
            List<double> highLevelExit = indicatorFactory.Highest(priceForChannelLowEntry, periodHighExit);
            List<double> lowLevelExit = indicatorFactory.Lowest(priceForChannelLowExit, periodLowExit);

            // Сглаживание
            int smoothPeriod = 5;
            highLevelEntry = indicatorFactory.Ema(highLevelEntry, smoothPeriod);
            lowLevelEntry = indicatorFactory.Ema(lowLevelEntry, smoothPeriod);
            highLevelExit = indicatorFactory.Ema(highLevelExit, smoothPeriod);
            lowLevelExit = indicatorFactory.Ema(lowLevelExit, smoothPeriod);

            // Сдвиг вправо на одну свечу
            highLevelEntry = highLevelEntry.Shift(1);
            lowLevelEntry = lowLevelEntry.Shift(1);
            highLevelExit = highLevelExit.Shift(1);
            lowLevelExit = lowLevelExit.Shift(1);

            // Переменные для обслуживания позиции
            double trailingStop = 0.0;

            for (int i = StabilizationPeriod; i < Candles.Count - 1; i++)
            {
                // Правило входа
                SignalLong = ClosePrices[i] > highLevelEntry[i];
                SignalShort = ClosePrices[i] < lowLevelEntry[i];

                // Задаем цену для заявки
                double orderPrice = Candles[i].Close;

                if (LastActivePosition == null)
                {
                    if (SignalLong)
                        BuyAtPrice(positionSize, orderPrice, i + 1);

                    else if (SignalShort) 
                        SellAtPrice(positionSize, orderPrice, i + 1);
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

                    else if (LastActivePosition.IsShort)
                    {
                        double startTrailingStop = highLevelExit[entryCandleIndex];
                        double curTrailingStop = highLevelExit[i];

                        trailingStop = i == entryCandleIndex
                            ? startTrailingStop
                            : Math.Min(trailingStop, curTrailingStop);

                        // Выход по стопу
                        CloseAtStop(LastActivePosition, trailingStop, i + 1);
                    }
                }
            }
        }
    }
}

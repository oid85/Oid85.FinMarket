using Oid85.FinMarket.Common.MathExtensions;
using Oid85.FinMarket.Strategies.Indicators;
using Oid85.FinMarket.Strategies.Indicators.Implementations;
using Oid85.FinMarket.Strategies.Models;

namespace Oid85.FinMarket.Strategies.Algorithms
{
    public class DonchianBreakoutClassicLongDaily : Strategy
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
            var priceForChannelHighEntry = HighPrices.Add(LowPrices)!.Add(ClosePrices)!.Add(ClosePrices)!.DivConst(4.0);
            var priceForChannelHighExit = HighPrices.Add(LowPrices)!.Add(ClosePrices)!.Add(ClosePrices)!.DivConst(4.0);
            var priceForChannelLowEntry = HighPrices.Add(LowPrices)!.Add(ClosePrices)!.Add(ClosePrices)!.DivConst(4.0);
            var priceForChannelLowExit = HighPrices.Add(LowPrices)!.Add(ClosePrices)!.Add(ClosePrices)!.DivConst(4.0);

            // Построение каналов
            var highLevelEntry = new HighestBand(priceForChannelHighEntry, periodHighEntry).Values;
            var lowLevelEntry = new LowestBand(priceForChannelHighExit, periodLowEntry).Values;
            var highLevelExit = new HighestBand(priceForChannelLowEntry, periodHighExit).Values;
            var lowLevelExit = new LowestBand(priceForChannelLowExit, periodLowExit).Values;

            // Сглаживание
            int smoothPeriod = 5;
            highLevelEntry = new Ema(highLevelEntry, smoothPeriod).Values;
            lowLevelEntry = new Ema(lowLevelEntry, smoothPeriod).Values;
            highLevelExit = new Ema(highLevelExit, smoothPeriod).Values;
            lowLevelExit = new Ema(lowLevelExit, smoothPeriod).Values;

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

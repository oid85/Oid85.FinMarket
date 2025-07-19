using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Common.MathExtensions;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Strategies
{
    public class DonchianBreakoutClassicShort(
        IIndicatorFactory indicatorFactory) 
        : Strategy
    {
        public override void Execute()
        {
            // Получаем параметры
            int periodEntry = Parameters["PeriodEntry"];
            int periodExit = Parameters["PeriodExit"];

            // Фильтр
            var filterEma = indicatorFactory.Ema(Candles, periodEntry);
            
            // Цены для построения канала
            List<double> price = HighPrices.Add(LowPrices)!.Add(ClosePrices)!.Add(ClosePrices)!.DivConst(4.0);

            // Построение каналов
            List<double> highLevel = indicatorFactory.Highest(price, periodExit);
            List<double> lowLevel = indicatorFactory.Lowest(price, periodEntry);

            // Сглаживание
            int smoothPeriod = 5;
            highLevel = indicatorFactory.Sma(highLevel, smoothPeriod);
            lowLevel = indicatorFactory.Sma(lowLevel, smoothPeriod);

            // Сдвиг вправо на одну свечу
            highLevel = highLevel.Shift(1);
            lowLevel = lowLevel.Shift(1);

            // Переменные для обслуживания позиции
            double trailingStop = 0.0;

            for (int i = StabilizationPeriod; i < Candles.Count - 1; i++)
            {
                // Правило входа
                SignalShort = ClosePrices[i] < lowLevel[i];
                FilterShort = Candles[i].Close < filterEma[i];
                
                // Задаем цену для заявки
                double orderPrice = Candles[i].Close;

                // Расчет размера позиции
                int positionSize = GetPositionSize(orderPrice);
                
                if (LastActivePosition is null)
                {
                    if (SignalShort && FilterShort)
                        SellAtPrice(positionSize, orderPrice, i + 1);
                }
                
                else
                {
                    int entryCandleIndex = LastActivePosition.EntryCandleIndex;

                    if (LastActivePosition.IsShort)
                    {
                        double startTrailingStop = highLevel[entryCandleIndex];
                        double curTrailingStop = highLevel[i];

                        trailingStop = i == entryCandleIndex ? startTrailingStop : Math.Min(trailingStop, curTrailingStop);
                        
                        if (Candles[i].Close >= trailingStop)
                            BuyAtPrice(positionSize, Candles[i].Close, i + 1);
                    }
                }
                
                // Отрисовка индикаторов
                GraphPoints[i].Filter = filterEma[i];
                GraphPoints[i].ChannelBands[0] = highLevel[i];
                GraphPoints[i].ChannelBands[1] = lowLevel[i];
            }
        }
    }
}

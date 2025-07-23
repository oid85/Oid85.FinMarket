using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Common.Utils;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Strategies
{
    public class BollingerBandsClassicShort(
        IIndicatorFactory indicatorFactory) 
        : Strategy
    {
        public override void Execute()
        {
            // Получаем параметры
            int periodEntry = Parameters["PeriodEntry"];
            int periodExit = Parameters["PeriodExit"];
            double stdDev = Parameters["StdDev"] / 10.0;

            // Фильтр
            var filterEma = indicatorFactory.Ema(Candles, periodEntry);
            
            // Построение каналов
            var bollingerBandsEntry = indicatorFactory.BollingerBands(Candles, periodEntry, stdDev);
            var bollingerBandsExit = indicatorFactory.BollingerBands(Candles, periodExit, stdDev);
            List<double> highLevel = bollingerBandsExit.UpperBand;
            List<double> lowLevel = bollingerBandsEntry.LowerBand;

            // Сдвиг вправо на одну свечу
            lowLevel = lowLevel.Shift(1);
            highLevel = highLevel.Shift(1);
            
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

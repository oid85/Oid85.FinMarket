using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Common.MathExtensions;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Strategies
{
    public class BollingerBandsMiddleShort(
        IIndicatorFactory indicatorFactory) 
        : Strategy
    {
        public override void Execute()
        {
            // Получаем параметры
            int period = Parameters["Period"];
            double stdDev = Parameters["StdDev"] / 10.0;

            // Фильтр
            var filterEma = indicatorFactory.Ema(Candles, period);
            
            // Построение каналов
            var bollingerBands = indicatorFactory.BollingerBands(Candles, period, stdDev);
            List<double> highLevel = bollingerBands.UpperBand;
            List<double> lowLevel = bollingerBands.LowerBand;

            // Сдвиг вправо на одну свечу
            highLevel = highLevel.Shift(1);
            lowLevel = lowLevel.Shift(1);
            
            // Средняя линия канала
            List<double> middleLine = highLevel.Add(lowLevel)!.DivConst(2.0);
            
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
                        double startTrailingStop = middleLine[entryCandleIndex];
                        double curTrailingStop = middleLine[i];

                        trailingStop = i == entryCandleIndex ? startTrailingStop : Math.Min(trailingStop, curTrailingStop);

                        if (Candles[i].Close >= trailingStop)
                            BuyAtPrice(positionSize, Candles[i].Close, i + 1);
                    }
                }
            }
        }
    }
}

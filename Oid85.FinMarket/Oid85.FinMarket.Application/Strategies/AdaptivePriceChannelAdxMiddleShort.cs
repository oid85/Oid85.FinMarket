using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Common.MathExtensions;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Strategies
{
    public class AdaptivePriceChannelAdxMiddleShort(
        IIndicatorFactory indicatorFactory) 
        : Strategy
    {
        public override void Execute()
        {
            // Получаем параметры
            int periodPc = Parameters["PeriodPc"];
            int periodAdx = Parameters["PeriodAdx"];

            // Фильтр
            var filterEma = indicatorFactory.Ema(Candles, periodPc);
            
            // Построение каналов
            var channel = indicatorFactory.AdaptivePriceChannelAdx(Candles, periodAdx, periodPc);
            
            List<double> highLevel = channel.UpperBand;
            List<double> lowLevel = channel.LowerBand;

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
                SignalShort = ClosePrices[i] < highLevel[i];
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

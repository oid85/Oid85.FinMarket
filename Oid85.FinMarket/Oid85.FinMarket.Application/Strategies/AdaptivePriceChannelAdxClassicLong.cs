﻿using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Common.MathExtensions;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Strategies
{
    public class AdaptivePriceChannelAdxClassicLong(
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
            
            // Переменные для обслуживания позиции
            double trailingStop = 0.0;

            for (int i = StabilizationPeriod; i < Candles.Count - 1; i++)
            {
                // Правило входа
                SignalLong = ClosePrices[i] > highLevel[i];
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
                        double startTrailingStop = lowLevel[entryCandleIndex];
                        double curTrailingStop = lowLevel[i];

                        trailingStop = i == entryCandleIndex ? startTrailingStop : Math.Max(trailingStop, curTrailingStop);
                        
                        if (Candles[i].Close <= trailingStop)
                            SellAtPrice(positionSize, Candles[i].Close, i + 1);
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

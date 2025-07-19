﻿using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Strategies
{
    public class SupertrendLong(
        IIndicatorFactory indicatorFactory) 
        : Strategy
    {
        public override void Execute()
        {
            // Получаем параметры
            int period = Parameters["Period"];
            double multiplier = Parameters["Multiplier"] / 10.0;
            
            // Фильтр
            var filterEma = indicatorFactory.Ema(Candles, period);
            
            // Расчет индикаторов
            List<double> supertrend = indicatorFactory.Supertrend(Candles, period, multiplier);

            for (int i = StabilizationPeriod; i < Candles.Count - 1; i++)
            {
                // Правило входа
                SignalLong = Candles[i].Close > supertrend[i];
                FilterLong = Candles[i].Close > filterEma[i];
                
                // Правило выхода
                SignalCloseLong = Candles[i].Close < supertrend[i];
                
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
                    if (SignalCloseLong)
                        SellAtPrice(positionSize, orderPrice, i + 1);
                }
                
                // Отрисовка индикаторов
                GraphPoints[i].Filter = filterEma[i];
                GraphPoints[i].Indicator = supertrend[i];
            }
        }
    }
}

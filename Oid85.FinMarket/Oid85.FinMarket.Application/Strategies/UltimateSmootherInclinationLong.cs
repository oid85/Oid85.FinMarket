﻿using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Strategies
{
    public class UltimateSmootherInclinationLong(
        IIndicatorFactory indicatorFactory) 
        : Strategy
    {
        public override void Execute()
        {
            // Получаем параметры
            int period = Parameters["Period"];
            
            // Расчет индикаторов
            List<double> ultimateSmoother = indicatorFactory.UltimateSmoother(ClosePrices, period);

            for (int i = StabilizationPeriod; i < Candles.Count - 1; i++)
            {
                // Правило входа
                SignalLong = 
                    ultimateSmoother[i - 2] > ultimateSmoother[i - 3] && 
                    ultimateSmoother[i - 1] > ultimateSmoother[i - 2] &&
                    ultimateSmoother[i] > ultimateSmoother[i - 1];

                // Правило выхода
                SignalCloseLong = 
                    ultimateSmoother[i - 2] < ultimateSmoother[i - 3] &&
                    ultimateSmoother[i - 1] < ultimateSmoother[i - 2] &&
                    ultimateSmoother[i] < ultimateSmoother[i - 1];
                
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
                GraphPoints[i].Indicator = ultimateSmoother[i];
            }
        }
    }
}

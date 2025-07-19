using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Strategies
{
    public class HmaInclinationShort(
        IIndicatorFactory indicatorFactory) 
        : Strategy
    {
        public override void Execute()
        {
            // Получаем параметры
            int period = Parameters["Period"];
            
            // Расчет индикаторов
            List<double> hma = indicatorFactory.Hma(Candles, period);

            for (int i = StabilizationPeriod; i < Candles.Count - 1; i++)
            {
                // Правило входа
                SignalShort = 
                    hma[i - 2] < hma[i - 3] &&
                    hma[i - 1] < hma[i - 2] &&
                    hma[i] < hma[i - 1];                    

                // Правило выхода
                SignalCloseShort = 
                    hma[i - 2] > hma[i - 3] && 
                    hma[i - 1] > hma[i - 2] &&
                    hma[i] > hma[i - 1];
                
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
                    if (SignalCloseShort)
                        BuyAtPrice(positionSize, orderPrice, i + 1);
                }
                
                // Отрисовка индикаторов
                GraphPoints[i].Indicator = hma[i];
            }
        }
    }
}

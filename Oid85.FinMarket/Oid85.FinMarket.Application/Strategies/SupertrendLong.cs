using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Common.MathExtensions;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Strategies
{
    public class SupertrendLong(
        IIndicatorFactory indicatorFactory) 
        : Strategy
    {
        public override void Execute()
        {
            int positionSize = 1;
            
            // Определяем параметра
            int period = Parameters["Period"];
            double multiplier = Parameters["Multiplier"] / 10.0;
            
            // Цена для анализа
            List<double> analysePrice = HighPrices.Add(LowPrices)!.Add(ClosePrices)!.Add(ClosePrices)!.DivConst(4.0);

            // Расчет индикаторов
            List<double> supertrend = indicatorFactory.Supertrend(Candles, period, multiplier);

            for (int i = StabilizationPeriod; i < Candles.Count - 1; i++)
            {
                // Правило входа
                SignalLong = analysePrice[i] > supertrend[i];

                // Правило выхода
                SignalCloseLong = analysePrice[i] < supertrend[i];
                
                // Задаем цену для заявки
                double orderPrice = Candles[i].Close;

                if (LastActivePosition == null)
                {
                    if (SignalLong)
                        BuyAtPrice(positionSize, orderPrice, i + 1);
                }
                
                else
                {
                    if (SignalCloseLong)
                        SellAtPrice(positionSize, orderPrice, i + 1);
                }
            }
        }
    }
}

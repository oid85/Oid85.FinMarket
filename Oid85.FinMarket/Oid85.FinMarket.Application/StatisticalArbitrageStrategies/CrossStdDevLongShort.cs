using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.StatisticalArbitrageStrategies;

public class CrossStdDevLongShort : StatisticalArbitrageStrategy
{
    public override void Execute()
    {
        // Получаем параметры
        double stdDev = Parameters["StdDev"] / 10.0;

        for (int i = 1; i < Candles.First.Count - 1; i++)
        {
            var date = DateOnly.FromDateTime(Candles.First[i].DateTime);
            var spread = Spreads.Find(x => x.Date == date);
            
            if (spread is null)
                continue;
            
            // Правило входа
            SignalLongShort = spread.Value >= stdDev;

            // Правило выхода
            SignalCloseLongShort = spread.Value <= 0.0;
                
            // Задаем цену для заявки
            var orderPrice = (Candles.First[i].Close, Candles.Second[i].Close);

            // Расчет размера позиции
            var positionSize = GetPositionSize(orderPrice);
                
            if (LastActivePosition is null)
            {
                if (SignalLongShort && FilterLongShort)
                    BuySellAtPrice(positionSize, orderPrice, i + 1);
            }
                
            else
            {
                if (SignalCloseLongShort)
                    SellBuyAtPrice(positionSize, orderPrice, i + 1);
            }
                
            // Отрисовка
            GraphPoints[i].PriceFirst = Candles.First[i].Close;
            GraphPoints[i].PriceSecond = Candles.Second[i].Close;
            GraphPoints[i].Spread = spread.Value;
        }
    }
}
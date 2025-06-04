using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class EhlersUnlinearFilterHelper : IndicatorHelper
    {
        public override IList<object> ParameterDefaultValues { get { return new object[] {BarDataType.Bars}; } }
        public override IList<string> ParameterDescriptions { get { return new string[] {"Источник"}; } }
        public override string TargetPane { get { return "P"; } }
        public override Color DefaultColor { get { return Color.DarkRed; } }
        public override Type IndicatorType { get { return typeof(EhlersUnlinearFilter); } }
        public override string Description { get { return "Нелинейный фильтр Эйлерса"; } }
        public override string URL { get { return @"http://www.bot4sale.ru/index.php?option=com_content&view=article&id=162&catid=15&Itemid=130"; } }
    }

    public class EhlersUnlinearFilter : DataSeries
    {
        public EhlersUnlinearFilter(Bars bars, string description)
            : base(bars, description)
        {
            DataSeries price = new DataSeries(bars.Close - bars.Close, @"price");
            DataSeries coef = new DataSeries(bars.Close - bars.Close, @"coef");
            DataSeries dcef = new DataSeries(bars.Close - bars.Close, @"dcef");

            price = (bars.High + bars.Low)/2;

            const int coefLookback = 5;

            FirstValidValue = Math.Max(FirstValidValue, coefLookback);
            for (int i = FirstValidValue; i < bars.Count; i++)
                coef[i] = Math.Pow(price[i] - price[i - 1], 2) +
                          Math.Pow(price[i] - price[i - 2], 2) +
                          Math.Pow(price[i] - price[i - 3], 2) +
                          Math.Pow(price[i] - price[i - 4], 2) +
                          Math.Pow(price[i] - price[i - 5], 2);

            double sumCoef = 0.0;
            double sumCoefPrice = 0.0;

            for (int i = FirstValidValue; i < bars.Count; i++)
            {
                for (int j = 0; j < coefLookback; j++)
                {
                    sumCoef += coef[i - j];
                    sumCoefPrice += (coef[i - j] * price[i - j]);
                }

                dcef[i] = sumCoefPrice/sumCoef;
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = dcef[bar];
        }

        /// <summary>
        /// Возвращает экзепляр индикатора
        /// </summary>
        /// <returns>Экземпляр индикатора: новый или из кеша</returns>
        public static EhlersUnlinearFilter Series(Bars bars)
        {
            const string description = "EhlersUnlinearFilter";
            if (bars.Cache.ContainsKey(description))
                return (EhlersUnlinearFilter) bars.Cache[description];
            var ehlersUnlinearFilter = new EhlersUnlinearFilter(bars, description);
            bars.Cache[description] = ehlersUnlinearFilter;
            return ehlersUnlinearFilter;
        }

    }
}
using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class NoTailTRHelper : IndicatorHelper
    {
        public override string Description { get { return @"Истинный диапазон (без хвостов)"; } }
        public override Type IndicatorType { get { return typeof(NoTailTR); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return "NoTailTR"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Red; } }
    }

    public class NoTailTR : DataSeries
    {
        public NoTailTR(Bars bars, string description)
            : base(bars, description)
        {
            FirstValidValue = 1;

            DataSeries ntHigh = bars.Close - bars.Close; // Максимумы "без хвостов"
            DataSeries ntLow = bars.Close - bars.Close;  // Минимумы "без хвостов"

            // "срезаем хвосты"

            for (int bar = 0; bar < bars.Count; bar++)
                if (bars.Close[bar] > bars.Open[bar]) // Если свеча белая
                {
                    ntHigh[bar] = bars.Close[bar];
                    ntLow[bar] = bars.Open[bar];
                }
                else // Если свечка черная
                {
                    ntHigh[bar] = bars.Open[bar];
                    ntLow[bar] = bars.Close[bar];
                }

            DataSeries ntRangeLow = DataSeries.Abs((bars.Close >> 1) - Lowest.Series(ntLow, 1));    // Абсолютное значение разницы текущего минимума и предыдущего закрытия ("без хвостов")
            DataSeries ntRangeHigh = DataSeries.Abs((bars.Close >> 1) - Highest.Series(ntHigh, 1)); // Абсолютное значение разницы текущего максимума и предыдущего закрытия ("без хвостов")

            for (int bar = 1; bar < bars.Count; bar++)
                this[bar] = new List<double> { ntRangeLow[bar], ntRangeHigh[bar] }.Max();
        }

        public static NoTailTR Series(Bars bars)
        {
            string description = String.Format("NoTailTR"); // Описание на графике
            if (bars.Cache.ContainsKey(description)) // Если индикатор есть в кеше
                return (NoTailTR)bars.Cache[description]; // то вернуть его из кеша
            var result = new NoTailTR(bars, description); // Иначе создаем индикатор
            bars.Cache[description] = result; // Заносим его в кеш
            return result; // Возвращаем его
        }
    }
}
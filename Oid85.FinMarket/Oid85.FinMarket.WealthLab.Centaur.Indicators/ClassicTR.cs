using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class ClassicTRHelper : IndicatorHelper
    {
        public override string Description { get { return @"Классический истинный диапазон"; } }
        public override Type IndicatorType { get { return typeof(ClassicTR); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return "ClassicTR"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Red; } }
    }

    public class ClassicTR : DataSeries
    {
        public ClassicTR(Bars bars, string description)
            : base(bars, description)
        {
            FirstValidValue = 1;

            DataSeries rangeLow = DataSeries.Abs((bars.Close >> 1) - Lowest.Series(bars.Low, 1));    // Абсолютное значение разницы текущего минимума и предыдущего закрытия
            DataSeries rangeHigh = DataSeries.Abs((bars.Close >> 1) - Highest.Series(bars.High, 1)); // Абсолютное значение разницы текущего максимума и предыдущего закрытия
            DataSeries rangeHighLow = bars.High - bars.Low; // Диапазон между максимумом и минимумом

            for (int bar = 1; bar < bars.Count; bar++)
                this[bar] = new List<double> {rangeLow[bar], rangeHigh[bar], rangeHighLow[bar]}.Max();
        }

        public static ClassicTR Series(Bars bars)
        {
            string description = String.Format("ClassicTR"); // Описание на графике
            if (bars.Cache.ContainsKey(description)) // Если индикатор есть в кеше
                return (ClassicTR)bars.Cache[description]; // то вернуть его из кеша
            var result = new ClassicTR(bars, description); // Иначе создаем индикатор
            bars.Cache[description] = result; // Заносим его в кеш
            return result; // Возвращаем его
        }
    }
}
using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class HighDailyHelper : IndicatorHelper
    {
        public override string Description { get { return @"HighDaily"; } }
        public override Type IndicatorType { get { return typeof(HighDaily); } }
        public override IList<string> ParameterDescriptions { get { return new[] {@"Источник"}; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
    }

    public class HighDaily : DataSeries
    {
        public HighDaily(Bars bars, string description)
            : base(bars, description)
        {
            var highDaily = new DataSeries(bars.Close - bars.Close, @"highDaily");

            for (int bar = 0; bar < bars.Count; bar++)
            {
                // Дата текущей свечи
                var dt = bars.Date[bar];

                var values = new List<double>();

                // Помещаем в массив свечи последнего дня
                for (int i = bar; i >= 0; i--)
                {
                    if (bars.Date[i].Date == dt.Date)
                    {
                        values.Add(bars.High[i]);
                    }
                    else
                    {
                        break;
                    }
                }

                highDaily[bar] = values.Max();
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = highDaily[bar];
        }

        public static HighDaily Series(Bars bars)
        {
            string description = String.Format("HighDaily");
            if (bars.Cache.ContainsKey(description))
                return (HighDaily)bars.Cache[description];
            var result = new HighDaily(bars, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
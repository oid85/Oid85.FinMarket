using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class LowDailyHelper : IndicatorHelper
    {
        public override string Description { get { return @"LowDaily"; } }
        public override Type IndicatorType { get { return typeof(LowDaily); } }
        public override IList<string> ParameterDescriptions { get { return new[] {@"Источник"}; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
    }

    public class LowDaily : DataSeries
    {
        public LowDaily(Bars bars, string description)
            : base(bars, description)
        {
            var lowDaily = new DataSeries(bars.Close - bars.Close, @"lowDaily");

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
                        values.Add(bars.Low[i]);
                    }
                    else
                    {
                        break;
                    }
                }

                lowDaily[bar] = values.Min();
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = lowDaily[bar];
        }

        public static LowDaily Series(Bars bars)
        {
            string description = String.Format("LowDaily");
            if (bars.Cache.ContainsKey(description))
                return (LowDaily)bars.Cache[description];
            var result = new LowDaily(bars, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
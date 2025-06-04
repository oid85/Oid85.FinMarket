using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class PercentileCannelUpHelper : IndicatorHelper
    {
        public override string Description { get { return @"PercentileCannelUp"; } }
        public override Type IndicatorType { get { return typeof(PercentileCannelUp); } }
        public override IList<string> ParameterDescriptions { get { return new[] {@"Источник", @"Период", @"Процентиль"}; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 3, 200), new RangeBoundInt32(75, 50, 100) }; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
        public override string URL { get { return @""; } }
    }

    public class PercentileCannelUp : DataSeries
    {
        public PercentileCannelUp(DataSeries ds, int period, double percent, string description)
            : base(ds, description)
        {
            FirstValidValue = period;

            if (ds.Count < period)
                return;

            for (int bar = 0; bar < ds.Count; bar++)
            {
                var values = new List<double>();

                for (int i = period; i >= 0; i--)
                    values.Add(ds[i]);

                values = new List<double>(values.OrderBy(v => v));

                int index = Convert.ToInt32(Math.Floor(values.Count * percent / 100.0));

                this[bar] = values[index];
            }
        }

        public static PercentileCannelUp Series(DataSeries ds, int period, double percent)
        {
            string description = String.Format("PercentileCannelUp: {0}, {1}", period, percent);
            if (ds.Cache.ContainsKey(description))
                return (PercentileCannelUp)ds.Cache[description];
            var percentileCannelUp = new PercentileCannelUp(ds, period, percent, description);
            ds.Cache[description] = percentileCannelUp;
            return percentileCannelUp;
        }
    }
}
using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class PercentileCannelDownHelper : IndicatorHelper
    {
        public override string Description { get { return @"PercentileCannelDown"; } }
        public override Type IndicatorType { get { return typeof(PercentileCannelDown); } }
        public override IList<string> ParameterDescriptions { get { return new[] { @"Источник", @"Период", @"Процентиль" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 3, 200), new RangeBoundInt32(75, 50, 100) }; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
        public override string URL { get { return @""; } }
    }

    public class PercentileCannelDown : DataSeries
    {
        public PercentileCannelDown(DataSeries ds, int period, double percent, string description)
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

        public static PercentileCannelDown Series(DataSeries ds, int period, double percent)
        {
            string description = String.Format("PercentileCannelDown: {0}, {1}", period, percent);
            if (ds.Cache.ContainsKey(description))
                return (PercentileCannelDown)ds.Cache[description];
            var percentileCannelDown = new PercentileCannelDown(ds, period, percent, description);
            ds.Cache[description] = percentileCannelDown;
            return percentileCannelDown;
        }
    }
}
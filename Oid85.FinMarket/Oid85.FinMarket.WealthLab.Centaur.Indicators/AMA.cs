using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class AMAHelper : IndicatorHelper
    {
        public override string Description { get { return @"AMA"; } }
        public override Type IndicatorType { get { return typeof(AMA); } }
        public override IList<string> ParameterDescriptions { get { return new[] { @"Источник", @"Период", @"Fast", @"Slow" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 3, 200), new RangeBoundInt32(2, 1, 10), new RangeBoundInt32(30, 1, 50) }; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
    }

    public class AMA : DataSeries
    {
        public AMA(DataSeries ds, int period, int fast, int slow, string description)
            : base(ds, description)
        {
            FirstValidValue = period + 2;
            int count = ds.Count;

            if (count < FirstValidValue)
                return;

            var ama = new DataSeries(ds - ds, @"AMA");


            double fastSmoothingConst = 2.0 / (double) (fast + 1);
            double slowSmoothingConst = 2.0 / (double) (slow + 1);

            double amaPrev = ds[0];

            for (int i = period + 2; i < count; i++)
            {
                double d1 = Math.Abs(ds[i] - ds[i - period]);
                double d2 = 0.0;
                for (int j = 0; j < period; j++)
                {
                    d2 += Math.Abs(ds[i - j] - ds[i - j - 1]);
                }
                double er = d1 / d2;
                double scc = er * (fastSmoothingConst - slowSmoothingConst) + slowSmoothingConst;
                double amaCur = amaPrev + Math.Pow(scc, 2.0) * (ds[i] - amaPrev);
                ama[i] = amaCur;
                amaPrev = amaCur;
            }

            for (int bar = 0; bar < ds.Count; bar++)
                this[bar] = ama[bar];

        }

        public static AMA Series(DataSeries ds, int period, int fast, int slow)
        {
            string description = String.Format("AMA: {0}, {1}, {2}", period, fast, slow);
            if (ds.Cache.ContainsKey(description))
                return (AMA)ds.Cache[description];
            var ama = new AMA(ds, period, fast, slow, description);
            ds.Cache[description] = ama;
            return ama;
        }
    }
}
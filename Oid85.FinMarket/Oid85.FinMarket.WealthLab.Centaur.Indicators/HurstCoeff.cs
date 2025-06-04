using System.Drawing;
using Oid85.FinMarket.Common.MathExtensions;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class HurstCoeffHelper : IndicatorHelper
    {
        public override string Description { get { return @"HurstCoeff"; } }
        public override Type IndicatorType { get { return typeof(HurstCoeff); } }
        public override IList<string> ParameterDescriptions { get { return new[] { @"Источник", @"Период" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 3, 200) }; } }
        public override string TargetPane { get { return @"HurstCoeff"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
        public override string URL { get { return @"http://research-journal.org/economical/pokazatel-xersta-kak-mera-fraktalnoj-struktury-i-dolgosrochnoj-pamyati-finansovyx-rynkov/"; } }
    }

    public class HurstCoeff : DataSeries
    {
        public HurstCoeff(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            FirstValidValue = period;
            int count = ds.Count;

            if (count < FirstValidValue)
                return;

            var hurst = new DataSeries(ds - ds, @"hurst");

            for (int i = period; i < count; i++)
            {
                var values = new List<double>();

                for (int j = i - period; j < i; j++)
                    values.Add(ds[j]);

                double stdDev = values.StdDev(); // Стандартное отклонение
                double r = values.Range(); // Размах
                double nr = r / stdDev; // Нормированный размах
                double logrs = System.Math.Log10(nr);
                double lognpi2 = System.Math.Log10(period * (System.Math.PI / 2.0));
                double h = logrs / lognpi2;
                double rst = nr * 0.998752 + 1.051037;
                double logrst = System.Math.Log10(rst);
                double ht = logrst / lognpi2 * (-0.0011 * System.Math.Log(period) + 1.0136);
                hurst[i] = ht;
            }

            for (int bar = 0; bar < ds.Count; bar++)
                this[bar] = hurst[bar];

        }

        public static HurstCoeff Series(DataSeries ds, int period)
        {
            string description = String.Format("HurstCoeff: {0}", period);
            if (ds.Cache.ContainsKey(description))
                return (HurstCoeff)ds.Cache[description];
            var hurst = new HurstCoeff(ds, period, description);
            ds.Cache[description] = hurst;
            return hurst;
        }
    }
}
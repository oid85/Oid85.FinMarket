using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class MACDHelper : IndicatorHelper
    {
        public override string Description { get { return @"MACD"; } }
        public override Type IndicatorType { get { return typeof(MACD); } }
        public override IList<string> ParameterDescriptions { get { return new[] { @"Источник", @"Fast Period EMA", @"Slow Period EMA" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 3, 200), new RangeBoundInt32(20, 3, 200) }; } }
        public override string TargetPane { get { return @"MACD"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Histogram; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
    }

    public class MACD : DataSeries
    {
        public MACD(DataSeries ds, int fast, int slow, string description)
            : base(ds, description)
        {
            FirstValidValue = new List<int> {fast, slow }.Max() * 3;

            if (ds.Count < FirstValidValue)
                return;

            var emaFast = EMA.Series(ds, fast, EMACalculation.Modern);
            var emaSlow = EMA.Series(ds, slow, EMACalculation.Modern);
            var macd = emaFast - emaSlow;

            for (int bar = 0; bar < ds.Count; bar++)
                this[bar] = macd[bar];
        }

        public static MACD Series(DataSeries ds, int fast, int slow)
        {
            string description = String.Format("MACD({0}, {1})", fast, slow);
            if (ds.Cache.ContainsKey(description))
                return (MACD)ds.Cache[description];
            var macd = new MACD(ds, fast, slow, description);
            ds.Cache[description] = macd;
            return macd;
        }
    }
}
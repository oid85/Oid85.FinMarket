using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class MACDSignalHelper : IndicatorHelper
    {
        public override string Description { get { return @"MACDSignal"; } }
        public override Type IndicatorType { get { return typeof(MACDSignal); } }
        public override IList<string> ParameterDescriptions { get { return new[] { @"Источник", @"Fast Period EMA", @"Slow Period EMA", @"Signal Period" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 3, 200), new RangeBoundInt32(20, 3, 200), new RangeBoundInt32(20, 3, 200) }; } }
        public override string TargetPane { get { return @"MACDSignal"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Red; } }
        public override int DefaultWidth { get { return 2; } }
    }

    public class MACDSignal : DataSeries
    {
        public MACDSignal(DataSeries ds, int fast, int slow, int signal, string description)
            : base(ds, description)
        {
            FirstValidValue = new List<int> {fast, slow, signal }.Max() * 3;

            if (ds.Count < FirstValidValue)
                return;

            var emaFast = EMA.Series(ds, fast, EMACalculation.Modern);
            var emaSlow = EMA.Series(ds, slow, EMACalculation.Modern);
            var macd = emaFast - emaSlow;
            var macdSignal = SMA.Series(macd, signal);

            for (int bar = 0; bar < ds.Count; bar++)
                this[bar] = macdSignal[bar];
        }

        public static MACDSignal Series(DataSeries ds, int fast, int slow, int signal)
        {
            string description = String.Format("MACDSignal({0}, {1}, {2})", fast, slow, signal);
            if (ds.Cache.ContainsKey(description))
                return (MACDSignal)ds.Cache[description];
            var macdSignal = new MACDSignal(ds, fast, slow, signal, description);
            ds.Cache[description] = macdSignal;
            return macdSignal;
        }
    }
}
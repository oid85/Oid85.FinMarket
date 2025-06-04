using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class AvgVolumeHelper : IndicatorHelper
    {
        public override string Description { get { return @"AvgVolume"; } }
        public override Type IndicatorType { get { return typeof(AvgVolume); } }
        public override IList<string> ParameterDescriptions { get { return new[] { @"Бары", @"MAPeriod" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars, new RangeBoundInt32(20, 3, 200) }; } }
        public override string TargetPane { get { return @"V"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkRed; } }
        public override int DefaultWidth { get { return 2; } }
    }

    public class AvgVolume : DataSeries
    {
        public AvgVolume(Bars bars, int maPeriod, string description)
            : base(bars, description)
        {
            FirstValidValue = maPeriod * 3;

            DataSeries avgVolume = SMA.Series(bars.Volume, maPeriod);

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = avgVolume[bar];
        }

        public static AvgVolume Series(Bars bars, int maPeriod)
        {
            string description = String.Format("AvgVolume: {0}", maPeriod);
            if (bars.Cache.ContainsKey(description))
                return (AvgVolume)bars.Cache[description];
            var result = new AvgVolume(bars, maPeriod, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
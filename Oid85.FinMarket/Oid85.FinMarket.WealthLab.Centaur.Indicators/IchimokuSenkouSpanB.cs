using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class IchimokuSenkouSpanBHelper : IndicatorHelper
    {
        public override string Description { get { return @"IchimokuSenkouSpanB"; } }
        public override Type IndicatorType { get { return typeof(IchimokuSenkouSpanB); } }
        public override IList<string> ParameterDescriptions { get { return new[] { @"Бары", @"T1", @"T2", @"T3" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars, new RangeBoundInt32(20, 3, 100), new RangeBoundInt32(20, 3, 100), new RangeBoundInt32(20, 3, 100) }; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
        public override string URL { get { return @"http://vpluse.net/indikator-Ichimoku/263-indikator-Ichimoku-osnovnye-ponyatiya-metodika-rascheta"; } }
    }

    public class IchimokuSenkouSpanB : DataSeries
    {
        public IchimokuSenkouSpanB(Bars bars, int t1, int t2, int t3, string description)
            : base(bars, description)
        {
            FirstValidValue = new List<int> { t1, t2, t3 }.Max();

            if (!(t1 < t2 && t2 < t3))
                return;

            var ichimokuSenkouSpanB = new DataSeries(bars.Close - bars.Close, @"IchimokuSenkouSpanB");

            ichimokuSenkouSpanB = (Highest.Series(bars.High, t3) + Lowest.Series(bars.Low, t3)) / 2.0;
            ichimokuSenkouSpanB = ichimokuSenkouSpanB >> t2;

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = ichimokuSenkouSpanB[bar];
        }

        public static IchimokuSenkouSpanB Series(Bars bars, int t1, int t2, int t3)
        {
            string description = String.Format("IchimokuSenkouSpanB: {0}, {1}, {2}", t1, t2, t3);
            if (bars.Cache.ContainsKey(description))
                return (IchimokuSenkouSpanB)bars.Cache[description];
            var result = new IchimokuSenkouSpanB(bars, t1, t2, t3, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
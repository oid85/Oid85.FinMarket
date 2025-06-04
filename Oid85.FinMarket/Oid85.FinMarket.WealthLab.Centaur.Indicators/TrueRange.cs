using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class TrueRangeHelper : IndicatorHelper
    {
        public override string Description { get {  return @"TrueRange"; } }
        public override Type IndicatorType { get { return typeof(TrueRange); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return "TrueRange"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkRed; } }        
    }

    public class TrueRange : DataSeries
    {
        public TrueRange(Bars bars, string description)
            : base(bars, description)
        {
            FirstValidValue = 1;

            var trueRange = new DataSeries(bars.Close - bars.Close, @"trueRange");

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                trueRange[bar] = Math.Max(bars.High[bar], bars.Close[bar - 1]) -
                                 Math.Min(bars.Low[bar], bars.Close[bar - 1]);
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = trueRange[bar];
        }

        public static TrueRange Series(Bars bars)
        {
            string description = String.Format("TrueRange");
            if (bars.Cache.ContainsKey(description))
                return (TrueRange)bars.Cache[description];
            var result = new TrueRange(bars, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class BarRangeHelper : IndicatorHelper
    {
        public override string Description { get {  return @"BarRange"; } }
        public override Type IndicatorType { get { return typeof(BarRange); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return "BarRange"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkRed; } }        
    }

    public class BarRange : DataSeries
    {
        public BarRange(Bars bars, string description)
            : base(bars, description)
        {
            FirstValidValue = 1;

            var barRange = new DataSeries(bars.Close - bars.Close, @"barRange");

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                barRange[bar] = Math.Abs(bars.High[bar - 1] - bars.Low[bar - 1]);
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = barRange[bar];
        }

        public static BarRange Series(Bars bars)
        {
            string description = String.Format("BarRange");
            if (bars.Cache.ContainsKey(description))
                return (BarRange)bars.Cache[description];
            var result = new BarRange(bars, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
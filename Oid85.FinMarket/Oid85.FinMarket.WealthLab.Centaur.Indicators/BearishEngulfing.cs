using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class BearishEngulfingHelper : IndicatorHelper
    {
        public override string Description { get { return @"BearishEngulfing"; } }
        public override Type IndicatorType { get { return typeof(BearishEngulfing); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return "BearishEngulfing"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkBlue; } }        
    }

    public class BearishEngulfing : DataSeries
    {
        public BearishEngulfing(Bars bars, string description)
            : base(bars, description)
        {
            var bearishEngulfing = new DataSeries(bars.Close - bars.Close, @"bearishEngulfing");

            for (int bar = 1; bar < bars.Count; bar++)
            {
                double open = bars.Open[bar];
                double prevOpen = bars.Open[bar - 1];
                double close = bars.Close[bar];
                double prevClose = bars.Close[bar - 1];

                if ((prevClose > prevOpen) && 
                    (open > close) && 
                    (open >= prevClose) && 
                    (prevOpen >= close) && 
                    ((open - close) > (prevClose - prevOpen)))
                {
                    bearishEngulfing[bar] = 1.0;
                }
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = bearishEngulfing[bar];
        }

        public static BearishEngulfing Series(Bars bars)
        {
            string description = String.Format("BearishEngulfing)");
            if (bars.Cache.ContainsKey(description))
                return (BearishEngulfing)bars.Cache[description];
            var result = new BearishEngulfing(bars, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
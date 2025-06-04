using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class BullishEngulfingHelper : IndicatorHelper
    {
        public override string Description { get { return @"BullishEngulfing"; } }
        public override Type IndicatorType { get { return typeof(BullishEngulfing); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return "BullishEngulfing"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkBlue; } }        
    }

    public class BullishEngulfing : DataSeries
    {
        public BullishEngulfing(Bars bars, string description)
            : base(bars, description)
        {
            var bullishEngulfing = new DataSeries(bars.Close - bars.Close, @"bullishEngulfing");

            for (int bar = 1; bar < bars.Count; bar++)
            {
                double open = bars.Open[bar];
                double prevOpen = bars.Open[bar - 1];
                double close = bars.Close[bar];
                double prevClose = bars.Close[bar - 1];

                if ((prevOpen > prevClose) && 
                    (close > open) && 
                    (close >= prevOpen) && 
                    (prevClose >= open) && 
                    ((close - open) > (prevOpen - prevClose)))
                {
                    bullishEngulfing[bar] = 1.0;
                }
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = bullishEngulfing[bar];
        }

        public static BullishEngulfing Series(Bars bars)
        {
            string description = String.Format("BullishEngulfing)");
            if (bars.Cache.ContainsKey(description))
                return (BullishEngulfing)bars.Cache[description];
            var result = new BullishEngulfing(bars, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class HammerHelper : IndicatorHelper
    {
        public override string Description { get { return @"Hammer"; } }
        public override Type IndicatorType { get { return typeof(Hammer); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return "Hammer"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkBlue; } }        
    }

    public class Hammer : DataSeries
    {
        public Hammer(Bars bars, string description)
            : base(bars, description)
        {
            var hammer = new DataSeries(bars.Close - bars.Close, @"hammer");

            for (int bar = 1; bar < bars.Count; bar++)
            {
                double H = bars.High[bar];
                double L = bars.Low[bar];
                double L1 = bars.Low[bar - 1];
                double L2 = bars.Low[bar - 2];
                double L3 = bars.Low[bar - 3];

                double O = bars.Open[bar];
                double C = bars.Close[bar];
                double CL = H - L;

                double BodyLow, BodyHigh;
                double Candle_WickBody_Percent = 0.9;
                double CandleLength = 12;

                if (O > C)
                {
                    BodyHigh = O;
                    BodyLow = C;
                }
                else
                {
                    BodyHigh = C;
                    BodyLow = O;
                }

                double LW = BodyLow - L;
                double UW = H - BodyHigh;
                double BLa = Math.Abs(O - C);
                double BL90 = BLa * Candle_WickBody_Percent;

                double pipValue = bars.SymbolInfo.Tick;

                if ((L <= L1) && (L < L2) && (L < L3))
                {
                    if (((LW / 2) > UW) && (LW > BL90) && (CL >= (CandleLength * pipValue)) && (O != C) && ((LW / 3) <= UW) && ((LW / 4) <= UW)/*&&(H<H1)&&(H<H2)*/)
                    {
                        hammer[bar] = 1.0;
                    }
                    if (((LW / 3) > UW) && (LW > BL90) && (CL >= (CandleLength * pipValue)) && (O != C) && ((LW / 4) <= UW)/*&&(H<H1)&&(H<H2)*/)
                    {
                        hammer[bar] = 1.0;
                    }
                    if (((LW / 4) > UW) && (LW > BL90) && (CL >= (CandleLength * pipValue)) && (O != C)/*&&(H<H1)&&(H<H2)*/)
                    {
                        hammer[bar] = 1.0;
                    }
                }
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = hammer[bar];
        }

        public static Hammer Series(Bars bars)
        {
            string description = String.Format("Hammer)");
            if (bars.Cache.ContainsKey(description))
                return (Hammer)bars.Cache[description];
            var result = new Hammer(bars, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
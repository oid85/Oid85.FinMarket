using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class ShootingStarHelper : IndicatorHelper
    {
        public override string Description { get { return @"ShootingStar"; } }
        public override Type IndicatorType { get { return typeof(ShootingStar); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return "ShootingStar"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkBlue; } }        
    }

    public class ShootingStar : DataSeries
    {
        public ShootingStar(Bars bars, string description)
            : base(bars, description)
        {
            var shootingStar = new DataSeries(bars.Close - bars.Close, @"shootingStar");

            for (int bar = 1; bar < bars.Count; bar++)
            {
                double L = bars.Low[bar];
                double H = bars.High[bar];
                double H1 = bars.High[bar - 1];
                double H2 = bars.High[bar - 2];
                double H3 = bars.High[bar - 3];

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

                if ((H >= H1) && (H > H2) && (H > H3))
                {
                    if (((UW / 2) > LW) && (UW > (2 * BL90)) && (CL >= (CandleLength * pipValue)) && (O != C) && ((UW / 3) <= LW) && ((UW / 4) <= LW))
                    {
                        shootingStar[bar] = 1.0;
                    }
                    if (((UW / 3) > LW) && (UW > (2 * BL90)) && (CL >= (CandleLength * pipValue)) && (O != C) && ((UW / 4) <= LW))
                    {
                        shootingStar[bar] = 1.0;
                    }
                    if (((UW / 4) > LW) && (UW > (2 * BL90)) && (CL >= (CandleLength * pipValue)) && (O != C))
                    {
                        shootingStar[bar] = 1.0;
                    }
                }
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = shootingStar[bar];
        }

        public static ShootingStar Series(Bars bars)
        {
            string description = String.Format("ShootingStar)");
            if (bars.Cache.ContainsKey(description))
                return (ShootingStar)bars.Cache[description];
            var result = new ShootingStar(bars, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
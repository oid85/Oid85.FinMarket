using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class DarkCloudHelper : IndicatorHelper
    {
        public override string Description { get { return @"DarkCloud"; } }
        public override Type IndicatorType { get { return typeof(DarkCloud); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return "DarkCloud"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkBlue; } }        
    }

    public class DarkCloud : DataSeries
    {
        public DarkCloud(Bars bars, string description)
            : base(bars, description)
        {
            var darkCloud = new DataSeries(bars.Close - bars.Close, @"darkCloud");

            for (int bar = 1; bar < bars.Count; bar++)
            {
                double L = bars.Low[bar];
                double H = bars.High[bar];

                double O = bars.Open[bar];
                double O1 = bars.Open[bar - 1];
                double C = bars.Close[bar];
                double C1 = bars.Close[bar - 1];
                double CL = H - L;

                double OC_HL;
                if ((H - L) != 0)
                {
                    OC_HL = (O - C) / (H - L);
                }
                else
                {
                    OC_HL = 0;
                }

                double Piercing_Line_Ratio = 0.5;
                double Piercing_Candle_Length = 10;

                if ((C1 > O1) && (((C1 + O1) / 2) > C) && (O > C) && (C > O1) && (OC_HL > Piercing_Line_Ratio) && ((CL >= Piercing_Candle_Length * bars.SymbolInfo.Tick)))
                {
                    darkCloud[bar] = 1.0;
                }
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = darkCloud[bar];
        }

        public static DarkCloud Series(Bars bars)
        {
            string description = String.Format("DarkCloud)");
            if (bars.Cache.ContainsKey(description))
                return (DarkCloud)bars.Cache[description];
            var result = new DarkCloud(bars, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
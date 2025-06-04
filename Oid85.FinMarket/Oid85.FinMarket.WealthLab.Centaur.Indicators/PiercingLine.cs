using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class PiercingLineHelper : IndicatorHelper
    {
        public override string Description { get { return @"PiercingLine"; } }
        public override Type IndicatorType { get { return typeof(PiercingLine); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return "PiercingLine"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkBlue; } }        
    }

    public class PiercingLine : DataSeries
    {
        public PiercingLine(Bars bars, string description)
            : base(bars, description)
        {
            var piercingLine = new DataSeries(bars.Close - bars.Close, @"piercingLine");

            for (int bar = 1; bar < bars.Count; bar++)
            {
                double L = bars.Low[bar];
                double H = bars.High[bar];

                double O = bars.Open[bar];
                double O1 = bars.Open[bar - 1];
                double C = bars.Close[bar];
                double C1 = bars.Close[bar - 1];
                double CL = H - L;

                double CO_HL;
                if ((H - L) != 0)
                {
                    CO_HL = (C - O) / (H - L);
                }
                else
                {
                    CO_HL = 0;
                }

                double Piercing_Line_Ratio = 0.5;
                double Piercing_Candle_Length = 10;

                if ((C1 < O1) && (((O1 + C1) / 2) < C) && (O < C) && (CO_HL > Piercing_Line_Ratio) && (CL >= (Piercing_Candle_Length * bars.SymbolInfo.Tick)))
                {
                    piercingLine[bar] = 1.0;
                }
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = piercingLine[bar];
        }

        public static PiercingLine Series(Bars bars)
        {
            string description = String.Format("PiercingLine)");
            if (bars.Cache.ContainsKey(description))
                return (PiercingLine)bars.Cache[description];
            var result = new PiercingLine(bars, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
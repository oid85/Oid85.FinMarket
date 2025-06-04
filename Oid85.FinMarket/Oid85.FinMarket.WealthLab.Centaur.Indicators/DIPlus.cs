using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class DIPlusHelper : IndicatorHelper
    {
        public override string Description { get {  return @"DIPlus"; } }
        public override Type IndicatorType { get { return typeof(DIPlus); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары", "Период" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 300) }; } }
        public override string TargetPane { get { return "DIPlus"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkBlue; } }
        public override string URL { get { return @"https://ru.wikipedia.org/wiki/%D0%A1%D0%B8%D1%81%D1%82%D0%B5%D0%BC%D0%B0_%D0%BD%D0%B0%D0%BF%D1%80%D0%B0%D0%B2%D0%BB%D0%B5%D0%BD%D0%BD%D0%BE%D0%B3%D0%BE_%D0%B4%D0%B2%D0%B8%D0%B6%D0%B5%D0%BD%D0%B8%D1%8F"; } }
    }

    public class DIPlus : DataSeries
    {
        public DIPlus(Bars bars, int period, string description)
            : base(bars, description)
        {
            FirstValidValue = period + 1;

            var diPlus = new DataSeries(bars.Close - bars.Close, @"diPlus");
            var plusDM = new DataSeries(bars.Close - bars.Close, @"plusDM");
            var tr = new DataSeries(bars.Close - bars.Close, @"tr");

            for (int bar = 1; bar < bars.Count; bar++)
            {
                double plusM = bars.High[bar] - bars.High[bar - 1];
                double minusM = bars.Low[bar - 1] - bars.Low[bar];

                double plusDMv = 0.0;

                if (plusM > minusM && plusM > 0.0)
                    plusDMv = plusM;

                if (plusM < minusM || plusM < 0.0)
                    plusDMv = 0.0;

                double trv = new List<double> { bars.High[bar] - bars.Low[bar], bars.High[bar] - bars.Close[bar - 1], bars.Close[bar - 1] - bars.Low[bar] }.Max();

                plusDM[bar] = plusDMv;
                tr[bar] = trv;
            }

            diPlus = EMA.Series(plusDM / tr, period, EMACalculation.Modern) * 100.0;

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = diPlus[bar];
        }

        public static DIPlus Series(Bars bars, int period)
        {
            string description = String.Format("DIPlus({0})", period);
            if (bars.Cache.ContainsKey(description))
                return (DIPlus)bars.Cache[description];
            var result = new DIPlus(bars, period, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
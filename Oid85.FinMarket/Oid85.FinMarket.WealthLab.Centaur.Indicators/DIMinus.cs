using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class DIMinusHelper : IndicatorHelper
    {
        public override string Description { get {  return @"DIMinus"; } }
        public override Type IndicatorType { get { return typeof(DIMinus); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары", "Период" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 300) }; } }
        public override string TargetPane { get { return "DIMinus"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkBlue; } }
        public override string URL { get { return @"https://ru.wikipedia.org/wiki/%D0%A1%D0%B8%D1%81%D1%82%D0%B5%D0%BC%D0%B0_%D0%BD%D0%B0%D0%BF%D1%80%D0%B0%D0%B2%D0%BB%D0%B5%D0%BD%D0%BD%D0%BE%D0%B3%D0%BE_%D0%B4%D0%B2%D0%B8%D0%B6%D0%B5%D0%BD%D0%B8%D1%8F"; } }
    }

    public class DIMinus : DataSeries
    {
        public DIMinus(Bars bars, int period, string description)
            : base(bars, description)
        {
            FirstValidValue = period + 1;

            var diMinus = new DataSeries(bars.Close - bars.Close, @"diMinus");
            var minusDM = new DataSeries(bars.Close - bars.Close, @"minusDM");
            var tr = new DataSeries(bars.Close - bars.Close, @"tr");

            for (int bar = 1; bar < bars.Count; bar++)
            {
                double plusM = bars.High[bar] - bars.High[bar - 1];
                double minusM = bars.Low[bar - 1] - bars.Low[bar];

                double minusDMv = 0.0;

                if (minusM < plusM || minusM < 0.0)
                    minusDMv = 0.0;

                if (minusM > plusM && minusM > 0.0)
                    minusDMv = minusM;

                double trv = new List<double> { bars.High[bar] - bars.Low[bar], bars.High[bar] - bars.Close[bar - 1], bars.Close[bar - 1] - bars.Low[bar] }.Max();

                minusDM[bar] = minusDMv;
                tr[bar] = trv;
            }

            diMinus = EMA.Series(minusDM / tr, period, EMACalculation.Modern) * 100.0;

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = diMinus[bar];
        }

        public static DIMinus Series(Bars bars, int period)
        {
            string description = String.Format("DIMinus({0})", period);
            if (bars.Cache.ContainsKey(description))
                return (DIMinus)bars.Cache[description];
            var result = new DIMinus(bars, period, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
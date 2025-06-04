using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class AdxHelper : IndicatorHelper
    {
        public override string Description { get {  return @"Adx"; } }
        public override Type IndicatorType { get { return typeof(Adx); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары", "Период" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 300) }; } }
        public override string TargetPane { get { return "Adx"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkRed; } }
        public override string URL { get { return @"https://ru.wikipedia.org/wiki/%D0%A1%D0%B8%D1%81%D1%82%D0%B5%D0%BC%D0%B0_%D0%BD%D0%B0%D0%BF%D1%80%D0%B0%D0%B2%D0%BB%D0%B5%D0%BD%D0%BD%D0%BE%D0%B3%D0%BE_%D0%B4%D0%B2%D0%B8%D0%B6%D0%B5%D0%BD%D0%B8%D1%8F"; } }
    }

    public class Adx : DataSeries
    {
        public Adx(Bars bars, int period, string description)
            : base(bars, description)
        {
            FirstValidValue = period + 1;

            var adx = new DataSeries(bars.Close - bars.Close, @"Adx");
            var dx = new DataSeries(bars.Close - bars.Close, @"Dx");

            var diPlus = DIPlus.Series(bars, period);
            var diMinus = DIMinus.Series(bars, period);

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                dx[bar] = Math.Abs(diPlus[bar] - diMinus[bar]) / (diPlus[bar] + diMinus[bar]);
            }

            adx = EMA.Series(dx, period, EMACalculation.Modern) * 100.0;

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = adx[bar];
        }

        public static Adx Series(Bars bars, int period)
        {
            string description = String.Format("Adx({0})", period);
            if (bars.Cache.ContainsKey(description))
                return (Adx)bars.Cache[description];
            var result = new Adx(bars, period, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
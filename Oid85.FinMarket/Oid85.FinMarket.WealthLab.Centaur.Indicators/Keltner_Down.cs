using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class Keltner_DownHelper : IndicatorHelper
    {
        public override string Description { get { return @"Keltner_Down"; } }
        public override Type IndicatorType { get { return typeof(Keltner_Down); } }
        public override IList<string> ParameterDescriptions { get { return new[] { @"Бары", @"MAPeriod", @"Koef" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars, new RangeBoundInt32(20, 3, 200), new RangeBoundDouble(1.5, 1, 10) }; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
        public override string URL { get { return @"https://ru.wikipedia.org/wiki/%D0%9A%D0%B0%D0%BD%D0%B0%D0%BB_%D0%9A%D0%B5%D0%BB%D1%8C%D1%82%D0%BD%D0%B5%D1%80%D0%B0"; } }
    }

    public class Keltner_Down : DataSeries
    {
        public Keltner_Down(Bars bars, int maPeriod, double koef, string description)
            : base(bars, description)
        {
            FirstValidValue = maPeriod * 3;

            DataSeries down = new DataSeries(bars.Close - bars.Close, @"down");
            DataSeries middle = SMA.Series(bars.Close, maPeriod);

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                double sum = 0.0;

                for (int i = bar; i > bar - maPeriod; i--)
                    sum += bars.High[i] - bars.Low[i];

                double avg = sum / maPeriod;

                down[bar] = middle[bar] - koef * avg;
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = down[bar];
        }

        public static Keltner_Down Series(Bars bars, int maPeriod, double koef)
        {
            string description = String.Format("Keltner_Down: {0}, {1}", maPeriod, koef);
            if (bars.Cache.ContainsKey(description))
                return (Keltner_Down)bars.Cache[description];
            var result = new Keltner_Down(bars, maPeriod, koef, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
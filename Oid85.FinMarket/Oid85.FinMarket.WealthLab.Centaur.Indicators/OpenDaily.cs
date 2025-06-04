using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class OpenDailyHelper : IndicatorHelper
    {
        public override string Description { get { return @"OpenDaily"; } }
        public override Type IndicatorType { get { return typeof(OpenDaily); } }
        public override IList<string> ParameterDescriptions { get { return new[] {@"Источник"}; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
    }

    public class OpenDaily : DataSeries
    {
        public OpenDaily(Bars bars, string description)
            : base(bars, description)
        {
            var openDaily = new DataSeries(bars.Close - bars.Close, @"openDaily");

            for (int bar = 0; bar < bars.Count; bar++)
            {
                // Дата текущей свечи
                var dt = bars.Date[bar];

                var values = new List<double>();

                // Помещаем в массив свечи последнего дня
                for (int i = bar; i >= 0; i--)
                {
                    if (bars.Date[i].Date == dt.Date)
                    {
                        values.Add(bars.Open[i]);
                    }
                    else
                    {
                        break;
                    }
                }

                openDaily[bar] = values.Last();
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = openDaily[bar];
        }

        public static OpenDaily Series(Bars bars)
        {
            string description = String.Format("OpenDaily");
            if (bars.Cache.ContainsKey(description))
                return (OpenDaily)bars.Cache[description];
            var result = new OpenDaily(bars, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
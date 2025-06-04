using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class CloseDailyHelper : IndicatorHelper
    {
        public override string Description { get { return @"CloseDaily"; } }
        public override Type IndicatorType { get { return typeof(CloseDaily); } }
        public override IList<string> ParameterDescriptions { get { return new[] {@"Источник"}; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
    }

    public class CloseDaily : DataSeries
    {
        public CloseDaily(Bars bars, string description)
            : base(bars, description)
        {
            var closeDaily = new DataSeries(bars.Close - bars.Close, @"closeDaily");

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
                        values.Add(bars.Close[i]);
                    }
                    else
                    {
                        break;
                    }
                }

                closeDaily[bar] = values.First();
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = closeDaily[bar];
        }

        public static CloseDaily Series(Bars bars)
        {
            string description = String.Format("CloseDaily");
            if (bars.Cache.ContainsKey(description))
                return (CloseDaily)bars.Cache[description];
            var result = new CloseDaily(bars, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
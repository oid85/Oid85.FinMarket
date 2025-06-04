using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class LowestInRangeHelper : IndicatorHelper
    {
        public override string Description { get { return @"LowestInRange"; } }
        public override Type IndicatorType { get { return typeof(LowestInRange); } }
        public override IList<string> ParameterDescriptions { get { return new[] { @"Источник", @"fromHour", @"fromMinute", @"toHour", @"toMinute" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars, new RangeBoundInt32(0, 0, 23), new RangeBoundInt32(0, 0, 59), new RangeBoundInt32(0, 0, 23), new RangeBoundInt32(0, 0, 59) }; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
    }

    public class LowestInRange : DataSeries
    {
        public LowestInRange(Bars bars, int fromHour, int fromMinute, int toHour, int toMinute, string description)
            : base(bars, description)
        {
            var lowestInRange = new DataSeries(bars.Low - bars.Low, @"LowestInRange");

            lowestInRange[0] = bars.Low[0];

            bool flag = false;

            for (int i = 1; i < bars.Count; i++)
            {
                if (TimeFit(bars.Date[i].Hour, bars.Date[i].Minute, fromHour, fromMinute, toHour, toMinute))
                {
                    if (flag)
                    {
                        if (bars.Low[i] < lowestInRange[i - 1])
                        {
                            lowestInRange[i] = bars.Low[i];
                        }
                        else
                        {
                            lowestInRange[i] = lowestInRange[i - 1];
                        }
                    }
                    else
                    {
                        lowestInRange[i] = bars.Low[i];
                        flag = true;
                    }
                }
                else
                {
                    lowestInRange[i] = lowestInRange[i - 1];
                    flag = false;
                }
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = lowestInRange[bar];
        }

        /// <summary>
        /// Входит ли заданное время в границы
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="fromHour"></param>
        /// <param name="fromMinute"></param>
        /// <param name="toHour"></param>
        /// <param name="toMinute"></param>
        /// <returns></returns>
        private bool TimeFit(int hour, int minute, int fromHour, int fromMinute, int toHour, int toMinute)
        {
            var date1 = DateTime.Now.Date.AddHours(fromHour).AddMinutes(fromMinute);
            var date2 = DateTime.Now.Date.AddHours(toHour).AddMinutes(toMinute);

            var date = DateTime.Now.Date.AddHours(hour).AddMinutes(minute);

            if (date1 >= date2)
            {
                date2 = date2.AddDays(-1);
                return date >= date2 && date <= date1;
            }

            return date >= date1 && date <= date2;
        }

        public static LowestInRange Series(Bars bars, int fromHour, int fromMinute, int toHour, int toMinute)
        {
            string description = String.Format("LowestInRange: {0}, {1}, {2}, {3}", fromHour, fromMinute, toHour, toMinute);
            if (bars.Cache.ContainsKey(description))
                return (LowestInRange)bars.Cache[description];
            var lowestInRange = new LowestInRange(bars, fromHour, fromMinute, toHour, toMinute, description);
            bars.Cache[description] = lowestInRange;
            return lowestInRange;
        }
    }
}
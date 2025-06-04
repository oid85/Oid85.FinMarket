using System.Drawing;
using Oid85.FinMarket.WealthLab.Centaur.Indicators.Types;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class PivotHelper : IndicatorHelper
    {
        public override string Description { get { return @"Pivot"; } }
        public override Type IndicatorType { get { return typeof(Pivot); } }
        public override IList<string> ParameterDescriptions { get { return new[] { @"Источник", @"Уровень" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars, PivotLevel.P }; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
    }

    public class Pivot : DataSeries
    {
        struct Candle
        {
            public double Open;
            public double Close;
            public double High;
            public double Low;
            public double Volume;
            public DateTime Date;
        }

        public Pivot(Bars bars, PivotLevel pivotLevel, string description)
            : base(bars, description)
        {
            // Т.к. индикатор будет для интрадея, то проверяем таймфрейм
            bool correctTimeFrame = bars.DataScale.Scale == BarScale.Minute && bars.DataScale.BarInterval <= 60; // Часовки и менее

            // Если таймфрейм не подходит, то выходим
            if (!correctTimeFrame)
                return;

            // Должны охватить не менее 2 дней
            FirstValidValue = (int)(Math.Ceiling((TimeSpan.FromDays(1).TotalMinutes / bars.DataScale.BarInterval)) * 2 * 1.1); // (число минут в сутках / размер бара в минутах + 10%, чтобы наверняка...)

            // Если баров меньше, чем нужно, то выходим
            if (bars.Count < FirstValidValue)
                return;

            DataSeries H = bars.Close - bars.Close;
            DataSeries L = bars.Close - bars.Close;
            DataSeries C = bars.Close - bars.Close;

            for (int i = FirstValidValue; i < bars.Count; i++)
            {
                List<Candle> candles = new List<Candle>();

                // Приведем все бары к типу Candle (чтобы потом использовать Linq)            
                for (int j = i; j > i - FirstValidValue; j--)
                    candles.Add(new Candle
                    {
                        Open = bars.Open[j],
                        Close = bars.Close[j],
                        High = bars.High[j],
                        Low = bars.Low[j],
                        Volume = bars.Volume[j],
                        Date = bars.Date[j]
                    });

                // Даты торговых дней
                List<DateTime> dates = candles.Select(d => d.Date.Date).Distinct().OrderByDescending(d => d).ToList();

                // Дата предыдущего торгового дня
                DateTime prevDay = dates[1].Date;
                
                List<Candle> prevDayCandles = candles.Where(c => c.Date.Date == prevDay.Date).ToList();

                if (prevDayCandles.Count == 0)
                    continue;

                // Сортируем по дате
                prevDayCandles.OrderBy(c => c.Date);

                // Берем максимум
                H[i] = prevDayCandles.Max(c => c.High);

                // Берем минимум
                L[i] = prevDayCandles.Min(c => c.Low);

                // Берем цену закрытия
                C[i] = prevDayCandles.First().Close;

                prevDayCandles.Clear();
                candles.Clear();
            }

            // Расчет уровней
            var pivot = new DataSeries(bars.Close - bars.Close, @"pivot");
            
            //  Уровень Pivot
            DataSeries P = (H + L + C) / 3;

            switch (pivotLevel)
            {
                case PivotLevel.P:
                    pivot = P;
                    break;

                case PivotLevel.R1:
                    pivot = (2 * P) - L;
                    break;

                case PivotLevel.S1:
                    pivot = (2 * P) - H;
                    break;

                case PivotLevel.R2:
                    pivot = P + H - L;
                    break;

                case PivotLevel.S2:
                    pivot = P - H + L;
                    break;

                case PivotLevel.R3:
                    pivot = H + 2 * (P - L);
                    break;

                case PivotLevel.S3:
                    pivot = H - 2 * (L - P);
                    break;
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = pivot[bar];
        }

        public static Pivot Series(Bars bars, PivotLevel pivotLevel)
        {
            string description = String.Format("Pivot: {0}", pivotLevel.ToString());
            if (bars.Cache.ContainsKey(description))
                return (Pivot)bars.Cache[description];
            var result = new Pivot(bars, pivotLevel, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
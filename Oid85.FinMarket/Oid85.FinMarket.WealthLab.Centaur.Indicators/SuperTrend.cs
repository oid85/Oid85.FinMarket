// При CCI(50)>0 индикатор суммирует максимум свечи и ATR(5), 
// а при CCI(50)<0 вычитает из минимума свечи значение ATR(5). 
// Получившаяся сумма или разница и является показанием индикатора.

using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class SuperTrendHelper : IndicatorHelper
    {
        public override string Description { get { return @"SuperTrend"; } }
        public override Type IndicatorType { get { return typeof (SuperTrend); } }
        public override IList<string> ParameterDescriptions { get { return new[] { @"Бары", @"Период CCI", @"Период ATR" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars, new RangeBoundInt32(50, 3, 200), new RangeBoundInt32(5, 3, 50) }; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkGreen; } }
        public override int DefaultWidth { get { return 2; } }
        public override string URL { get { return @"http://vpluse.net/polzovatelskie-indikatory/627-trendovyj-indikator-supertrend"; } }
    }

    public class SuperTrend : DataSeries
    {
        public SuperTrend(Bars bars, int periodCci, int periodAtr, string description)
            : base(bars, description)
        {
            FirstValidValue = Math.Max(periodAtr * 3, periodCci * 3);

            if (bars.Count < FirstValidValue)
                return;

            var atr = ATR.Series(bars, periodAtr);
            var cci = CCI.Series(bars, periodCci);

            DataSeries supeTrend = bars.Close - bars.Close;

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                if (cci[bar] >= 0)
                {
                    supeTrend[bar] = bars.High[bar] + atr[bar];

                    if (supeTrend[bar] < supeTrend[bar - 1])
                        supeTrend[bar] = supeTrend[bar - 1];
                }

                if (cci[bar] < 0)
                {
                    supeTrend[bar] = bars.Low[bar] - atr[bar];

                    if (supeTrend[bar] > supeTrend[bar - 1])
                        supeTrend[bar] = supeTrend[bar - 1];
                } 
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = supeTrend[bar];
        }

        public static SuperTrend Series(Bars bars, int periodCci, int periodAtr)
        {
            string description = String.Format("SupeTrend: {0}, {1}", periodCci, periodAtr);
            if (bars.Cache.ContainsKey(description))
                return (SuperTrend)bars.Cache[description];
            var supeTrend = new SuperTrend(bars, periodCci, periodAtr, description);
            bars.Cache[description] = supeTrend;
            return supeTrend;
        }
    }
}
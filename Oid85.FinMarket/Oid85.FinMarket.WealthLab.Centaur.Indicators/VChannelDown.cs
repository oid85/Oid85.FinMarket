using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class VChannelDownHelper : IndicatorHelper
    {
        public override string Description { get { return @"Volatility Channel Down"; } }
        public override Type IndicatorType { get { return typeof(VChannelDown); } }
        public override IList<string> ParameterDescriptions { get { return new[] {@"Источник", @"Период", @"Коэфф"}; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars, new RangeBoundInt32(50, 5, 300), new RangeBoundDouble(0.5, 3, 0.1) }; } }
        public override string TargetPane { get { return @"P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override int DefaultWidth { get { return 2; } }
        public override string URL { get { return @""; } }
    }

    public class VChannelDown : DataSeries
    {
        public VChannelDown(Bars bars, int period, double koeff, string description)
            : base(bars, description)
        {
            FirstValidValue = period;
            
            if (bars.Count < period)
                return;

            // ATR
            DataSeries atr = Atr.Series(bars, period);
            FirstValidValue = Math.Max(FirstValidValue, period * 4);

            // Расчетные цены, от которых будет откладывать волатильность
            DataSeries price = (bars.Open + bars.Close) / 2.0;

            // Граница канала волатильности
            DataSeries down = Lowest.Series(price + atr * koeff, period) >> 1;

            // Сглаживание
            down = EMA.Series(down, 5, EMACalculation.Modern);

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = down[bar];
        }

        public static VChannelDown Series(Bars bars, int period, double koeff)
        {
            string description = String.Format("VChannelDown: {0}, {1}", period, koeff);

            if (bars.Cache.ContainsKey(description))
                return (VChannelDown)bars.Cache[description];

            var down = new VChannelDown(bars, period, koeff, description);
            bars.Cache[description] = down;

            return down;
        }
    }
}
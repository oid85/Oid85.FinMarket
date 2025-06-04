using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class QQE_ADVHelper : IndicatorHelper
    {
        public override string Description { get {  return @"QQE_ADV"; } }
        public override Type IndicatorType { get { return typeof(QQE_ADV); } }
        public override IList<string> ParameterDescriptions { get { return new[] { "Бары", "SF", "RSI_Period" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars, new RangeBoundInt32(1, 1, 10), new RangeBoundInt32(8, 5, 20) }; } }
        public override string TargetPane { get { return "QQE_ADV"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkRed; } }        
    }

    public class QQE_ADV : DataSeries
    {
        public QQE_ADV(Bars bars, int sf, int rsiPeriod, string description)
            : base(bars, description)
        {
            FirstValidValue = rsiPeriod * 2;

            DataSeries rsi = RSI.Series(bars.Close, rsiPeriod);
            DataSeries rsiMa = EMA.Series(rsi, sf, EMACalculation.Modern);

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = rsiMa[bar];
        }

        public static QQE_ADV Series(Bars bars, int sf, int rsiPeriod)
        {
            string description = String.Format("QQE_ADV({0}, {1})", sf, rsiPeriod);
            if (bars.Cache.ContainsKey(description))
                return (QQE_ADV)bars.Cache[description];
            var result = new QQE_ADV(bars, sf, rsiPeriod, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
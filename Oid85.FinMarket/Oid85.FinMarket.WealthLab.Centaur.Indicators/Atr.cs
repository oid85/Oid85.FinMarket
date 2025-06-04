using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class AtrHelper : IndicatorHelper
    {
        public override string Description { get {  return @"Средний истинный диапазон"; } }
        public override Type IndicatorType { get {  return typeof(Atr); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары", "Период" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 300) }; } }
        public override string TargetPane { get { return "Atr"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkRed; } }
    }

    public class Atr : DataSeries
    {
        public Atr(Bars bars, int period, string description)
            : base(bars, description)
        {
            FirstValidValue = period + 1;

            // серия значений ATR
            var atr = new DataSeries(bars.Close - bars.Close, @"Atr");

			double trueRange = bars.High[0] - bars.Low[0];
			double vAtr = trueRange;

			for (int bar = 0; bar < bars.Count; bar++)
			{
				trueRange = bars.High[bar] - bars.Low[bar];
				if (bar > 0) 
				{
					if (bars.Low[bar] > bars.Close[bar - 1])
						trueRange	= trueRange + (bars.Low[bar] - bars.Close[bar - 1]);
					if (bars.High[bar] < bars.Close[bar - 1])
						trueRange	= trueRange + (bars.Close[bar - 1] - bars.High[bar]);
				}
				vAtr = vAtr + (trueRange - vAtr) / period;

				// добавление нового значения в последовательность
				atr[bar] = vAtr;
			}

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = atr[bar];
        }

        public static Atr Series(Bars bars, int period)
        {
            string description = String.Format("Atr({0})", period); // Описание на графике
            if (bars.Cache.ContainsKey(description)) // Если индикатор есть в кеше
                return (Atr)bars.Cache[description]; // то вернуть его из кеша
            var result = new Atr(bars, period, description); // Иначе создаем индикатор
            bars.Cache[description] = result; // Заносим его в кеш
            return result; // Возвращаем его
        }
    }
}
using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class AdaptiveParabolicHelper : IndicatorHelper
    {
        public override string Description { get { return @"Индикатор Parabolic учитывающий волатильность"; } }
        public override Type IndicatorType { get {  return typeof(AdaptiveParabolic); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары", "Период" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 300) }; } }
        public override string TargetPane { get { return "P"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Dots; } }
        public override int DefaultWidth { get { return 2; } }
        public override Color DefaultColor { get { return Color.DarkRed; } }
    }

    public class AdaptiveParabolic : DataSeries
    {
        public AdaptiveParabolic(Bars bars, int period, string description)
            : base(bars, description)
        {
            FirstValidValue = period;

            int oldBar = 0;
            bool lng = false;
            bool shrt = false;
            bool revers = false;

            double tr = 0.0;
            double atr = 0.0;
            double hmax = 0.0;
            double lmin = 0.0;
            double oldAtr = 0.0;
            double af = 0.0;

            var psar = new DataSeries(bars.Close - bars.Close, @"atrParabolic");

            for (int bar = 0; bar < bars.Count; bar++)
            {
	            if (bar < period)
		            psar[bar] = 0.0;
	            else
	            {
		            if (bar == period)
		            {
		                psar[bar] = bars.Low[bar];
		                lng = true;
			            hmax = bars.High[bar];
		                tr = 0.0;

                        for (int j = (bar - period); j < bar - 1; j++)
			            {
			                tr += bars.High[j] - bars.Low[j];
			            }

                        oldAtr = tr / period;
		                revers = true;
		            }
		            else
		            {
			            if (bar != oldBar)
			            {
			                tr = 0.0;

                            for (int j = (bar - period); j < bar - 1; j++)
				            {
				                tr += bars.High[j] - bars.Low[j];
				            }

                            atr = tr / period;
			                af = atr / (oldAtr + atr);
			                af = af / 10.0;
			                oldAtr = atr;

				            if (lng)
				            {
					            if (hmax < bars.High[bar - 1])
					            {
					                hmax = bars.High[bar - 1];
					            }

				                psar[bar] = psar[bar - 1] + af * (hmax - psar[bar - 1]);
				            }

				            if (shrt)
				            {
					            if (lmin > bars.Low[bar - 1])
					            {
					                lmin = bars.Low[bar - 1];
					            }

				                psar[bar] = psar[bar - 1] + af * (lmin - psar[bar - 1]);
				            }

			                revers = true;
			            }
	
			            if (lng && bars.Low[bar] < psar[bar] && revers)
			            {
			                psar[bar] = hmax;
			                shrt = true;
			                lng = false;
			                lmin = bars.Low[bar];
			                revers = false;
			            }
	
			            if (shrt && bars.High[bar] > psar[bar] && revers)
			            {
			                psar[bar] = lmin;
			                lng = true;
			                shrt = false;
			                hmax = bars.High[bar];
			                revers = false;
			            }
		            }

	                oldBar = bar;
	            }
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = psar[bar];
        }

        public static AdaptiveParabolic Series(Bars bars, int period)
        {
            string description = String.Format("AtrParabolic({0})", period); // Описание на графике
            if (bars.Cache.ContainsKey(description)) // Если индикатор есть в кеше
                return (AdaptiveParabolic)bars.Cache[description]; // то вернуть его из кеша
            var result = new AdaptiveParabolic(bars, period, description); // Иначе создаем индикатор
            bars.Cache[description] = result; // Заносим его в кеш
            return result; // Возвращаем его
        }
    }
}
using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class SRVIHelper : IndicatorHelper
    {
        public override string Description { get { return  @"Медленный индекс силы объема от Vitali Apirine"; } }
        public override string URL { get { return  @"http://www2.wealth-lab.com/WL5Wiki/SVSI.ashx"; } }
        public override Type IndicatorType { get { return  typeof(SRVI); } }
        public override IList<string> ParameterDescriptions{ get { return  new[] { "Бары", "Период SRVI", "Период WilderMA" }; } }
        public override IList<object> ParameterDefaultValues { get { return  new object[] { BarDataType.Bars, new RangeBoundInt32(6, 2, 300), new RangeBoundInt32(14, 2, 300) }; } }
        public override string TargetPane { get { return  "SRVI"; } }
        public override LineStyle DefaultStyle { get { return  LineStyle.Solid; } }
        public override Color DefaultColor { get { return  Color.Coral; } }
        public override bool IsOscillator { get { return  true; } }
        public override double OscillatorOversoldValue { get { return  20; } }
        public override double OscillatorOverboughtValue { get { return  80; } }
        public override Color OscillatorOversoldColor { get { return  Color.Red; } }
        public override Color OscillatorOverboughtColor { get { return  Color.Blue; } }
    }

    public class SRVI : DataSeries
    {
        public SRVI(Bars Bars, int SRVIPeriod, int WMAPeriod, string Description)
            : base(Bars, Description)
        {
            FirstValidValue = Math.Max(SRVIPeriod, WMAPeriod); // Начинаем индикатор с максимального значения периода

            var ema = EMA.Series(Bars.Close, SRVIPeriod, EMACalculation.Modern); // EMA по ценам закрытия
            var positiveDifference = new DataSeries(Bars, String.Format(@"PositiveDifference({0}, {1})", SRVIPeriod, WMAPeriod));
            var negativeDifference = new DataSeries(Bars, String.Format(@"NegativeDifference({0}, {1})", SRVIPeriod, WMAPeriod));

            for (int bar = FirstValidValue; bar < Bars.Count; bar++) // Пробегаемся по всем барам
            {
                positiveDifference[bar] = Bars.Close[bar] > ema[bar] ? Bars.Volume[bar] : 0; // Если закрытие выше EMA, то значение объема, иначе, 0
                negativeDifference[bar] = Bars.Close[bar] < ema[bar] ? Bars.Volume[bar] : 0; // Если закрытие ниже EMA, то значение объема, иначе, 0
            }

            var wmaPositive = WilderMA.Series(positiveDifference, WMAPeriod); // Позитивные и негативные разницы
            var wmaNegative = WilderMA.Series(negativeDifference, WMAPeriod); // сглаживаем WilderMA

            for (int bar = FirstValidValue; bar < Bars.Count; bar++) // Пробегаемся по всем барам
                this[bar] = wmaNegative[bar] == 0 ? 100 : 100 - 100 / (1 + wmaPositive[bar] / wmaNegative[bar]); // Формула RSI с защитой от деления на 0
        }

        public static SRVI Series(Bars Bars, int SRVIPeriod, int WMAPeriod)
        {
            string description = String.Format("SRVI({0}, {1})", SRVIPeriod, WMAPeriod); // Описание на графике
            if (Bars.Cache.ContainsKey(description)) // Если индикатор есть в кеше
                return (SRVI)Bars.Cache[description]; // то вернуть его из кеша
            var result = new SRVI(Bars, SRVIPeriod, WMAPeriod, description); // Иначе создаем индикатор
            Bars.Cache[description] = result; // Заносим его в кеш
            return result; // Возвращаем его
        }
    }
}
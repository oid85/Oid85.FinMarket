using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class SRSIHelper : IndicatorHelper
    {
        public override string Description { get { return @"Медленный RSI от Vitali Apirine"; } }
        public override string URL { get { return @"http://www2.wealth-lab.com/WL5Wiki/SRSI.ashx"; } }
        public override Type IndicatorType { get { return typeof(SRSI); } }
        public override IList<string> ParameterDescriptions{ get { return new[] { "Источник", "Период SRSI", "Период WilderMA" }; } }
        public override IList<object> ParameterDefaultValues{ get { return new object[] { CoreDataSeries.Close, new RangeBoundInt32(6, 2, 300), new RangeBoundInt32(14, 2, 300) }; } }
        public override string TargetPane { get { return "SRSI"; } }
        public override LineStyle DefaultStyle { get { return  LineStyle.Solid; } }
        public override Color DefaultColor { get { return  Color.Black; } }
        public override bool IsOscillator { get { return  true; } }
        public override double OscillatorOversoldValue { get { return  20; } }
        public override double OscillatorOverboughtValue { get { return  80; } }
        public override Color OscillatorOversoldColor { get { return  Color.Red; } }
        public override Color OscillatorOverboughtColor { get { return  Color.Blue; } }
    }

    public class SRSI : DataSeries
    {
        public SRSI(DataSeries DS, int SRSIPeriod, int WMAPeriod, string Description)
            : base(DS, Description)
        {
            FirstValidValue = Math.Max(SRSIPeriod, WMAPeriod); // Начинаем индикатор с максимального значения периода

            var ema = EMA.Series(DS, SRSIPeriod, EMACalculation.Modern);
            var positiveDifference = new DataSeries(DS, String.Format(@"PositiveDifference({0}, {1}, {2})", DS.Description, SRSIPeriod, WMAPeriod));
            var negativeDifference = new DataSeries(DS, String.Format(@"NegativeDifference({0}, {1}, {2})", DS.Description, SRSIPeriod, WMAPeriod));
            
            for (int bar = FirstValidValue; bar < DS.Count; bar++) // Пробегаемся по всем барам
            {
                positiveDifference[bar] = DS[bar] > ema[bar] ? DS[bar] - ema[bar] : 0; // Если закрытие выше EMA, то значение их разницы, иначе, 0
                negativeDifference[bar] = DS[bar] < ema[bar] ? ema[bar] - DS[bar] : 0; // Если закрытие ниже EMA, то значение их разницы, иначе, 0
            }

            var wmaPositive = WilderMA.Series(positiveDifference, WMAPeriod); // Позитивные и негативные разницы
            var wmaNegative = WilderMA.Series(negativeDifference, WMAPeriod); // сглаживаем WilderMA

            for (int bar = FirstValidValue; bar < DS.Count; bar++) // Пробегаемся по всем барам
                this[bar] = wmaNegative[bar] == 0 ? 100 : 100 - 100 / (1 + wmaPositive[bar] / wmaNegative[bar]); // Формула RSI с защитой от деления на 0
        }

        public static SRSI Series(DataSeries DS, int SRSIPeriod, int WMAPeriod)
        {
            string description = String.Format(@"SRSI({0}, {1}, {2})", DS.Description, SRSIPeriod, WMAPeriod); // Описание на графике
            if (DS.Cache.ContainsKey(description)) // Если индикатор есть в кеше
                return (SRSI)DS.Cache[description]; // то вернуть его из кеша
            var result = new SRSI(DS, SRSIPeriod, WMAPeriod, description); // Иначе создаем индикатор
            DS.Cache[description] = result; // Заносим его в кеш
            return result; // Возвращаем его
        }
    }
}
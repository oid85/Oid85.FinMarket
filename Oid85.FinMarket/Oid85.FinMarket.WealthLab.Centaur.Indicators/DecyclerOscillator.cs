using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class DecyclerOscillatorHelper : IndicatorHelper
    {
        public override string Description { get { return @"Простой расцикливатель от John Ehlers"; } }
        public override string URL { get { return @"http://www2.wealth-lab.com/WL5WIKI/TASCSep2015.ashx"; } }
        public override Type IndicatorType { get { return typeof(DecyclerOscillator); } }
        public override IList<string> ParameterDescriptions { get { return new[] { "Источник", "Период", "K" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { CoreDataSeries.Close, new RangeBoundInt32(125, 2, 300), new RangeBoundDouble(1, 1, 10) }; } }
        public override string TargetPane { get { return "DecyclerOscillator"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkRed; } }
        public override bool IsOscillator { get { return true; } }
    }

    /// <summary>
    /// Осциллятор расцикливания. Без задержки выделяет сверхнизкие частоты
    /// </summary>
    /// <remarks>Пересечение 2-х линий осциллятора. Оригинальная с Period и K=1, а также вспомогательная с Reriod*0.8 и K=1.2</remarks>
    public class DecyclerOscillator : DataSeries
    {
        public DecyclerOscillator(DataSeries DS, int Period, double K, string Description)
            : base(DS, Description)
        {
            FirstValidValue = 20; // Расцикливатели начинаются со 2-го бара, но их надо стабилизировать
            var sd = SimpleDecycler.Series(DS, Period); // Простой расцикливатель высоких частот с периодом
            var decyclerOscillator = Roofing.HighpassFilter(sd, (int)(Period / 2d)); // Применяем дополнительный фильтр высоких частот с полупериодом
            for (int bar = 0; bar < DS.Count; bar++) // Пробегаемся по всем барам
                this[bar] = 100d * K * decyclerOscillator[bar] / DS[bar]; // Формула осциллятора
        }

        public static DecyclerOscillator Series(DataSeries DS, int Period, double K)
        {
            string description = String.Format("DecyclerOscillator({0}, {1}, {2})", DS.Description, Period, K); // Описание на графике
            if (DS.Cache.ContainsKey(description)) // Если индикатор есть в кеше
                return (DecyclerOscillator)DS.Cache[description]; // то вернуть его из кеша
            var result = new DecyclerOscillator(DS, Period, K, description); // Иначе создаем индикатор
            DS.Cache[description] = result; // Заносим его в кеш
            return result; // Возвращаем его
        }
    }
}
using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class SimpleDecyclerHelper : IndicatorHelper
    {
        public override string Description { get { return @"Осциллятор расцикливания от John Ehlers"; } }
        public override string URL { get { return @"http://www2.wealth-lab.com/WL5WIKI/TASCJan2014.ashx"; } }
        public override Type IndicatorType { get { return typeof(SimpleDecycler); } }
        public override IList<string> ParameterDescriptions { get { return new[] { "Источник", "Период" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { CoreDataSeries.Close, new RangeBoundInt32(125, 2, 300) }; } }
        public override string TargetPane { get { return "P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkRed; } }
    }

    /// <summary>
    /// Простой расцикливатель
    /// </summary>
    public class SimpleDecycler : DataSeries
    {
        public SimpleDecycler(DataSeries DS, int Period, string Description)
            : base(DS, Description)
        {
            FirstValidValue = 15; // И SS и HPS начинаются со 2-го бара, но для них нужна стабилизация, т.к. используется обратная связь
            var highPassFilter = Roofing.HighpassFilter(DS, Period); // Фильтр высоких частот
            for (int bar = 2; bar < DS.Count; bar++) // Пробегаемся по всем барам
                this[bar] = DS[bar] - highPassFilter[bar]; // Фильтр низких частот = Цена - фильтр высоких частот
        }

        public static SimpleDecycler Series(DataSeries DS, int Period)
        {
            string description = String.Format("SimpleDecycler({0}, {1})", DS.Description, Period); // Описание на графике
            if (DS.Cache.ContainsKey(description)) // Если индикатор есть в кеше
                return (SimpleDecycler)DS.Cache[description]; // то вернуть его из кеша
            var result = new SimpleDecycler(DS, Period, description); // Иначе создаем индикатор
            DS.Cache[description] = result; // Заносим его в кеш
            return result; // Возвращаем его
        }
    }
}
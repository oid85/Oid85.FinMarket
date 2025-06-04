// Автор: Чечет Игорь Александрович
// Статья с описанием торговой системы: http://chechet.org/72
// Бесплатные курсы по трейдингу: http://chechet.org/learn/free/

using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class CTRHelper : IndicatorHelper
    {
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkRed; } }
        public override string TargetPane { get { return "CTR"; } }
        public override string Description { get { return "TR без учета эмоций (High, Low) рынка"; } }
        public override Type IndicatorType { get { return typeof(CTR); } }
        public override IList<object> ParameterDefaultValues { get { return new object[] {BarDataType.Bars}; } }
        public override IList<string> ParameterDescriptions { get { return new string[] {"Источник"}; } }
        public override string URL { get { return @"http://chechet.org/72"; } }
    }

    public class CTR : DataSeries
    {
        /// <summary>
        /// Конструктор индикатора
        /// </summary>
        public CTR(Bars bars, string description)
            : base(bars, description)
        {
            FirstValidValue = 1; // Индикатор будет построен со 2-ого бара, т.к. нужны данные предыдущего бара

            for (int bar = FirstValidValue; bar < bars.Count; bar++) // Пробегаемся по всем барам
                this[bar] = Math.Max(Math.Abs(bars.Close[bar] - bars.Open[bar]), // Максимальное значение из волатильности текущего дня
                    Math.Max(Math.Abs(bars.Close[bar] - bars.Close[bar - 1]), // от закрытия предыдущего дня до закрытия текущего дня
                    Math.Abs(bars.Open[bar] - bars.Close[bar - 1]))); // и от закрытия предыдущего дня до открытия текущего дня
        }

        /// <summary>
        /// Возвращает экзепляр индикатора
        /// </summary>
        /// <returns>Экземпляр индикатора: новый или из кеша</returns>
        public static CTR Series(Bars bars)
        {
            const string description = "CTR"; // Описание на графике
            if (bars.Cache.ContainsKey(description)) // Если индикатор есть в кеше
                return (CTR)bars.Cache[description]; // то вернуть его из кеша
            var ctr = new CTR(bars, description); // Создаем индикатор
            bars.Cache[description] = ctr; // Заносим его в кеш
            return ctr; // возвращаем его
        }
    }
}
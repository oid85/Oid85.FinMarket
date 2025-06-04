using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class CMAHelper : IndicatorHelper
    {
        public override IList<object> ParameterDefaultValues { get { return new object[] {BarDataType.Bars}; } }
        public override IList<string> ParameterDescriptions { get { return new string[] {"Источник"}; } }
        public override string TargetPane { get { return "P"; } }
        public override Color DefaultColor { get { return Color.DarkRed; } }
        public override Type IndicatorType { get { return typeof(CMA); } }
        public override string Description { get { return "Скользящая средняя, резко реагирующая на импульсы и мало реагирующая на флэт"; } }
        public override string URL { get { return @"http://chechet.org/74"; } }
    }

    public class CMA : DataSeries
    {
        const int Period = 100; // Кол-во элементов в списке значений CTR
        const double DivSize = 2; // На сколько равных частей делим текущий диапазон
        const double DivCount = 4; // Сколько раз делим диапазоны

        /// <summary>
        /// Сортированный список значений CTR
        /// </summary>
        private struct SortedCTR
        {
            /// <summary>
            /// Номер бара
            /// </summary>
            public int Bar;
            /// <summary>
            /// Значение CTR
            /// </summary>
            public double CTR;
        }
        /// <summary>
        /// Список процентов движений в зависимости от позиции в списке
        /// </summary>
        private struct MovePercent
        {
            /// <summary>
            /// Позиция в списке
            /// </summary>
            public int Id;
            /// <summary>
            /// Процент движения
            /// </summary>
            public double Percent;
        }
        /// <summary>
        /// Конструктор индикатора
        /// </summary>
        public CMA(Bars bars, string description)
            : base(bars, description)
        {
            #region Создаем список интервалов

            var movePercentList = new List<MovePercent>(); // Список интервалов
            var movePercentItem = new MovePercent(); // Элемент списка интервалов
            int movePercentIndex = 0; // Номер элемента списка
            int currentId = 0; // Индекс начала текущего интервала

            for (int i = 0; i <= DivCount; i++) // Интервалов будет на 1 больше, чем кол-во деления диапазонов
            {
                movePercentItem.Id = currentId; // Начальная позиция в сортированном списке CTR, к которому будет применен процент движения
                movePercentItem.Percent = 100 / Math.Pow(DivSize, DivCount - i); // Процент движения
                movePercentList.Add(movePercentItem); // Добавить интервал
                currentId += (int)((Period - currentId) / DivSize); // Индекс начала следующего интервала
            }

            #endregion

            DataSeries ctr = CTR.Series(bars); // TR без эмоций (High и Low) рынка

            #region Заполняем начальный список CTR

            var sortedCTRList = new List<SortedCTR>(); // Список значений CTR
            var ctrItem = new SortedCTR(); // Элемент списка значений CTR
            for (int bar = 1; bar < Period + 1; bar++) // Создаем начальный список значений CTR
            {
                ctrItem.Bar = bar;
                ctrItem.CTR = ctr[bar];
                sortedCTRList.Add(ctrItem);
            }

            #endregion

            FirstValidValue = Period; // Номер бара, с которого существует индикатор

            this[FirstValidValue] = bars.Close[FirstValidValue]; // Первое значение индикатора - цена закрытия бара

            for (int bar = Period + 1; bar < bars.Count; bar++) // Пробегаемся по всем барам
            {
                #region Удаляем самое старое значение из списка CTR

                int ctrIndex = SearchCTRById(sortedCTRList, bar - Period); // Найти номер элемента самого старого значения в списке
                ctrItem = sortedCTRList[ctrIndex]; // Получить этот элемент
                sortedCTRList.Remove(ctrItem); // Удалить его из списка CTR

                #endregion

                #region Добавляем новое значение в список CTR

                ctrItem.Bar = bar; // Текущий бар
                ctrItem.CTR = ctr[bar]; // Текущее значение CTR
                sortedCTRList.Add(ctrItem); // Добавить новые значения бара и CTR в список

                #endregion

                #region Сортируем список CTR

                sortedCTRList.Sort(
                    (a, b) => a.CTR.CompareTo(b.CTR));

                #endregion

                ctrIndex = SearchCTRById(sortedCTRList, bar); // Найти номер текущего элемента в списке

                #region Находим текущее движение в списке

                for (int i = 0; i < movePercentList.Count; i++) // Пробегаемся по всему списку движений
                {
                    if (ctrIndex >= movePercentList[i].Id) // Если мы входим в интервал
                        movePercentIndex = i; // то запомнить в какой интервал мы вошли
                    else // Если мы не входим в интервал
                        break; // то интервал найден, выходим
                }

                #endregion

                this[bar] = this[bar - 1] + // К прошлому значению CMA
                    (bars.Close[bar] - this[bar - 1]) * // прибавляем разницу между текущим закрытием бара и прошлым значением CMA
                    movePercentList[movePercentIndex].Percent / 100; // умноженное на процент движения, соответствующее интервалу
            }
        }

        /// <summary>
        /// Возвращает экзепляр индикатора
        /// </summary>
        /// <returns>Экземпляр индикатора: новый или из кеша</returns>
        public static CMA Series(Bars bars)
        {
            const string description = "CMA"; // Описание на графике
            if (bars.Cache.ContainsKey(description)) // Если индикатор есть в кеше
                return (CMA)bars.Cache[description]; // то вернуть его из кеша
            var cma = new CMA(bars, description); // Создаем индикатор
            bars.Cache[description] = cma; // Заносим его в кеш
            return cma; // возвращаем его
        }
        /// <summary>
        /// Поиск элемента в списке CTR по его Id
        /// </summary>
        /// <param name="sortedCtrList">Список CTR</param>
        /// <param name="id">Id элемента</param>
        /// <returns>Номер элемента в списке</returns>
        private int SearchCTRById(List<SortedCTR> sortedCtrList, int id)
        {
            for (int i = 0; i < sortedCtrList.Count; i++) // Пробегаемся по всем элементам списка
                if (sortedCtrList[i].Bar == id) // Если найден элемент по Id
                    return i; // Вернуть номер элемента в списке
            return -1; // Ошибка, значение не найдено
        }
    }
}
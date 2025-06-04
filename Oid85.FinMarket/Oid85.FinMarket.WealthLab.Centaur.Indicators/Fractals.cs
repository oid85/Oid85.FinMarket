using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class FractalsHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = { BarDataType.Bars };
        private static string[] _paramDescriptions = { "Источник" };
        public override LineStyle DefaultStyle { get { return LineStyle.Histogram; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override string TargetPane { get { return "Fractals"; } } 
        public override string Description { get { return "Фракталы"; } }
        public override Type IndicatorType { get { return typeof(Fractals); } }
        public override IList<object> ParameterDefaultValues { get { return _paramDefaults; } }
        public override IList<string> ParameterDescriptions { get { return _paramDescriptions; } }
    }

    /// <summary>
    /// Возвращает значения фракталов
    ///  0 - нет фрактала
    ///  1 - фрактал вверх
    ///  2 - максимум между двумя фракталами вниз (нетипичный фрактал вверх)
    /// -1 - фрактал вниз
    /// -2 - минимум между двумя фракталами вверх (нетипичный фрактал вниз)
    /// </summary>
    public class Fractals : DataSeries
    {
        struct FractalBar
        {
            public int Bar;
            public double FractalValue;
        }

        /// <summary>
        /// Конструктор индикатора
        /// </summary>
        /// <param name="Bars">Свечки</param>
        /// /// <param name="Description">Описание</param>
        public Fractals(Bars Bars, string Description)
            : base(Bars, Description)
        {
            FractalBar lastFractal;
            lastFractal.Bar = 0;
            lastFractal.FractalValue = 0.0d;

            this.FirstValidValue = 4; // Раз исследуем 5 баров, то начинаем с завершенного с индексом 4
            for (int bar = 2; bar < Bars.Count - 2; bar++)
            {
                #region Фракталы
                
                // Фрактал вверх
                if (Bars.High[bar - 2] < Bars.High[bar] &&
                    Bars.High[bar - 1] < Bars.High[bar] &&
                    Bars.High[bar + 1] < Bars.High[bar] &&
                    Bars.High[bar + 2] < Bars.High[bar])
                    this[bar] = 1;

                // Фрактал вниз
                if (Bars.Low[bar - 2] > Bars.Low[bar] &&
                    Bars.Low[bar - 1] > Bars.Low[bar] &&
                    Bars.Low[bar + 1] > Bars.Low[bar] &&
                    Bars.Low[bar + 2] > Bars.Low[bar])
                    this[bar] = -1;

                #endregion

                #region Нетипичные фракталы

                if (this[bar] != 0) // Если на данной свечке получен фрактал
                {
                    if (lastFractal.FractalValue != 0 && Math.Sign(lastFractal.FractalValue) == Math.Sign(this[bar])) // и этот фрактал совпадает по типу (знаку) с предыдущим фракталом
                    {
                        int nonTypicalFractalBar = lastFractal.Bar + 1; // Нетипичный фрактал устанавливаем сначала на следующей свечке за предыдущим фракталом

                        if (this[bar] > 0) // Для фракталов вверх
                        {
                            for (int i = lastFractal.Bar + 1; i < bar; i++) // Пробегаемся по всем свечкам от предыдущего максимума до текущего
                                if (Bars.Low[i] < Bars.Low[nonTypicalFractalBar]) // Если найдена свечка с минимальным значением
                                    nonTypicalFractalBar = i; // то запоминаем ее
                            this[nonTypicalFractalBar] = -2; // Устанавливаем нетипичный фрактал вниз
                        }
                        else // Для фракталов вниз
                        {
                            for (int i = lastFractal.Bar + 1; i < bar; i++) // Пробегаемся по всем свечкам от предыдущего минимума до текущего
                                if (Bars.High[i] > Bars.High[nonTypicalFractalBar]) // Если найдена свечка с максимальным значением
                                    nonTypicalFractalBar = i; // то запоминаем ее
                            this[nonTypicalFractalBar] = 2; // Устанавливаем нетипичный фрактал вверх
                        }
                    }
                    lastFractal.Bar = bar; // Запоминаем номер центра фрактала
                    lastFractal.FractalValue = this[bar]; // и его значение
                }

                #endregion
            }
        }

        /// <summary>
        /// Возвращает экзепляр индикатора
        /// </summary>
        /// <param name="Bars">Свечки</param>
        /// <returns>Экземпляр индикатора: новый или из кеша</returns>
        public static Fractals Series(Bars Bars)
        {
            string description = "Фракталы"; // Описание на графике
            if (Bars.Cache.ContainsKey(description)) // Если индикатор есть в кеше
                return (Fractals)Bars.Cache[description]; // то вернуть его из кеша

            Fractals fractals = new Fractals(Bars, description); // Создаем индикатор
            Bars.Cache[description] = fractals; // Заносим его в кеш
            return fractals; // возвращаем его
        }
    }
}

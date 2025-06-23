namespace Oid85.FinMarket.Common.MathExtensions
{
    /// <summary>
    /// Математические операции над рядами чисел
    /// </summary>
    public static class ListDoubleExtensions
    {
        /// <summary>
        /// Сдвиг вправо
        /// </summary>
        /// <param name="values"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        public static List<double> Shift(this List<double> values, int shift)
        {
            var result = new List<double>();
            
            for (int i = 0; i < shift; i++)
                result.Add(0.0);

            for (int i = 0; i < values.Count - shift; i++)
                result.Add(values[i]);

            return result;
        }
        
        /// <summary>
        /// Инициализация коллекции нулями
        /// </summary>
        public static List<double> InitValues(this List<double> list, int n)
        {
            for (int i = 0; i < n; i++)
                list.Add(0.0);

            return list;
        }

        /// <summary>
        /// Произведение всех членов
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static double Mult(this List<double> list) => 
            list.Aggregate(1.0, (current, t) => current * t);

        /// <summary>
        /// Умножение двух рядов
        /// </summary>
        /// <param name="list"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static List<double>? Mult(this List<double> list, List<double> mult) => 
            list.Count() != mult.Count() ? null : list.Select((t, i) => t * mult[i]).ToList();

        /// <summary>
        /// Умножение ряда на константу
        /// </summary>
        /// <param name="list"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static List<double> MultConst(this List<double> list, double mult) => 
            list.Select((t, i) => t * mult).ToList();

        /// <summary>
        /// Сложение двух рядов
        /// </summary>
        /// <param name="list"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        public static List<double>? Add(this List<double> list, List<double> add) => 
            list.Count != add.Count ? null : list.Select((t, i) => t + add[i]).ToList();

        /// <summary>
        /// Добавление константу к каждому элементу ряда
        /// </summary>
        /// <param name="list"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        public static List<double> AddConst(this List<double> list, double add) => 
            list.Select((t, i) => t + add).ToList();

        /// <summary>
        /// Вычитание из одного ряда другого
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sub"></param>
        /// <returns></returns>
        public static List<double>? Sub(this List<double> list, List<double> sub) => 
            list.Count != sub.Count ? null : list.Select((t, i) => t - sub[i]).ToList();

        /// <summary>
        /// Вычитание константы из каждого элемента
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sub"></param>
        /// <returns></returns>
        public static List<double> SubConst(this List<double> list, double sub) => 
            list.Select((t, i) => t - sub).ToList();

        /// <summary>
        /// Деление одного ряда на другой
        /// </summary>
        /// <param name="list"></param>
        /// <param name="div"></param>
        /// <returns></returns>
        public static List<double>? Div(this List<double> list, List<double> div) => 
            list.Count != div.Count ? null : list.Select((t, i) => t / div[i]).ToList();

        /// <summary>
        /// Деление ряда на константу
        /// </summary>
        /// <param name="list"></param>
        /// <param name="div"></param>
        /// <returns></returns>
        public static List<double> DivConst(this List<double> list, double div) => 
            list.Select((t, i) => t / div).ToList();

        /// <summary>
        /// Возведение ряда в степень
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pow"></param>
        /// <returns></returns>
        public static List<double> Pow(this List<double> list, double pow) => 
            list.Select(t => System.Math.Pow(t, pow)).ToList();
        
        /// <summary>
        /// Дисперсия
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static double Variance(this List<double> list)
        {
            double average = list.Average();
            double sum = list.Sum(t => (t - average) * (t - average));
            return sum / (list.Count - 1);
        }
        
        /// <summary>
        /// Стандартное отклонение
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static double StdDev(this List<double> list) => 
            Math.Sqrt(list.Variance());
        
        /// <summary>
        /// Размах
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static double Range(this List<double> list) => 
            list.Max() - list.Min();
    }
}

namespace Oid85.FinMarket.Common.MathExtensions
{
    /// <summary>
    /// Расчет статистических величин
    /// </summary>
    public static class Statistics
    {
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
        /// Корреляция по формуле Пирсона
        /// </summary>
        /// <param name="list"></param>
        /// <param name="correlation"></param>
        /// <returns></returns>
        public static double Correlation(this List<double> list, List<double> correlation)
        {
            if (list.Count != correlation.Count)
                return 0;

            double averageX = list.Average();
            double averageY = correlation.Average();

            double sum = 0.0;
            double sumX2 = 0.0;
            double sumY2 = 0.0;

            for (int i = 0; i < list.Count; i++)
            {
                sum += (list[i] - averageX) * (correlation[i] - averageY);
                sumX2 += (list[i] - averageX) * (list[i] - averageX);
                sumY2 += (correlation[i] - averageY) * (correlation[i] - averageY);
            }

            return sum / (System.Math.Sqrt(sumX2 * sumY2));
        }
        
        /// <summary>
        /// Ковариация
        /// </summary>
        /// <param name="list"></param>
        /// <param name="covariance"></param>
        /// <returns></returns>
        public static double Covariance(this List<double> list, List<double> covariance)
        {
            if (list.Count != covariance.Count)
                return 0;

            double averageX = list.Average();
            double averageY = covariance.Average();

            double sum = 0.0;

            for (int i = 0; i < list.Count; i++)
                sum += (list[i] - averageX) * (covariance[i] - averageY);

            return sum / (list.Count - 1);
        }

        /// <summary>
        /// Размах
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static double Range(this List<double> list) => 
            list.Max() - list.Min();

        /// <summary>
        /// Среднее геометрическое
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static double GeometricMean(this List<double> list) => 
            Math.Pow(list.Mult(), 1.0 / list.Count);
    }
}

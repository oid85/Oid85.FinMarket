namespace Oid85.FinMarket.Common.MathExtensions
{
    /// <summary>
    /// Расчет регрессий
    /// </summary>
    public static class Regression
    {
        /// <summary>
        /// Определение коэффициентов линейной регрессии.
        /// Находим зависимость значений в ряде с индексом 0 от значений в остальных рядах
        /// </summary>
        /// <param name="values">Ряды значений X и Y</param>
        /// <returns></returns>
        public static double[] GetCoefficients(List<List<double>> values)
        {
            alglib.linearmodel model;
            double[] coeffs;

            double[,] vals = new double[values.First().Count, values.Count];

            for (int i = 0; i < values.Count; i++)
                for (int j = 0; j < values[i].Count; j++)
                    vals[j, i] = values[i][j];

            int lenght = values.First().Count;
            int n = values.Count;

            alglib.lrbuild(vals, lenght, n - 1, out _, out model, out _);
            alglib.lrunpack(model, out coeffs, out _);

            return coeffs;
        }
    }
}

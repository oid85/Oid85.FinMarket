using System.Drawing;

namespace Oid85.FinMarket.Common.MathExtensions
{
    /// <summary>
    /// Класс с функциями, использующими генерацию случайных чисел
    /// </summary>
    public static class Rand
    {
        /// <summary>
        /// Генерация массива случайных цветов
        /// </summary>
        /// <param name="n">Размер массива</param>
        /// <returns></returns>
        public static List<Color> RandomColors(int n)
        {
            var colors = new List<Color>();
            var rnd = new Random(System.DateTime.Now.Millisecond);

            for (int i = 0; i < n; i++)
                colors.Add(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));

            return colors;
        }
    }
}

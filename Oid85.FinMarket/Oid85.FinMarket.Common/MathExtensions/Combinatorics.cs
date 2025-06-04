namespace Oid85.FinMarket.Common.MathExtensions
{
    /// <summary>
    /// Функции комбинаторики
    /// </summary>
    public static class Combinatorics
    {
        /// <summary>
        /// Определение сочетаний из n по m без повторений
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static IEnumerable<int[]> Combinations(int m, int n)
        {
            int[] result = new int[n];
            var stack = new Stack<int>();
            stack.Push(0);

            while (stack.Count > 0)
            {
                int index = stack.Count - 1;
                int value = stack.Pop();

                while (value < m)
                {
                    result[index++] = ++value;
                    stack.Push(value);

                    if (index == n)
                    {
                        yield return result;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Массив сочетаний из множества строк strings по n без повторений
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static List<List<string>> StringCombinations(List<string> strings, int n)
        {
            var result = new List<List<string>>();

            foreach (int[] combination in Combinations(strings.Count, n))
            {
                result.Add(new List<string>());

                foreach (int i in combination)
                {
                    result.Last().Add(strings[i - 1]);
                }
            }

            return result;
        }
    }
}

using System.Collections;
using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class ThreeCandlesPatternCodeHelper : IndicatorHelper
    {
        public override string Description { get { return @"Нормализация паттернов"; } }
        public override Type IndicatorType { get { return typeof(ThreeCandlesPatternCode); } }
        public override IList<string> ParameterDescriptions { get { return new[] {@"Источник"}; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] {BarDataType.Bars}; } }
        public override string TargetPane { get { return @"ThreeCandlesPatternCode"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Histogram; } }
        public override Color DefaultColor { get { return Color.DarkRed; } }
    }

    public class ThreeCandlesPatternCode : DataSeries
    {
        public ThreeCandlesPatternCode(Bars bars, string description)
            : base(bars, description)
        {
            FirstValidValue = 3;

            // Коды всех паттернов
            var codes = new List<long>();

            for (int i = 0; i < FirstValidValue; i++)
                codes.Add(0);

            // Сбор паттернов и присвоение кода
            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                var openPrices = new List<double>();
                var closePrices = new List<double>();
                var highPrices = new List<double>();
                var lowPrices = new List<double>();

                for (int i = FirstValidValue - 1; i >= 0; i--)
                {
                    openPrices.Add(bars.Open[bar - i]);
                    closePrices.Add(bars.Close[bar - i]);
                    highPrices.Add(bars.High[bar - i]);
                    lowPrices.Add(bars.Low[bar - i]);
                }

                // Код текущего паттерна
                string code = GetPatternCode(openPrices, closePrices, highPrices, lowPrices, Math.Pow(0.1, bars.SymbolInfo.Decimals));
                var intArray = new int[1]; // Определяем вспомогательную переменную для копирования массива битов

                var bits = new BitArray(17, false); // 17-и битный массив для кодирования паттерна (131 072 вариантов)

                // Преобразуем в двоичный код
                // Первая группа
                if (code[0] == '1' && code[1] == '1' && code[2] == '1') { bits[0] = false; bits[1] = false; bits[2] = false; bits[3] = false; bits[4] = false; }
                if (code[0] == '1' && code[1] == '1' && code[2] == '2') { bits[0] = false; bits[1] = false; bits[2] = false; bits[3] = false; bits[4] = true;  }
                if (code[0] == '1' && code[1] == '1' && code[2] == '3') { bits[0] = false; bits[1] = false; bits[2] = false; bits[3] = true;  bits[4] = false; }
                if (code[0] == '1' && code[1] == '2' && code[2] == '1') { bits[0] = false; bits[1] = false; bits[2] = false; bits[3] = true;  bits[4] = true;  }
                if (code[0] == '1' && code[1] == '2' && code[2] == '2') { bits[0] = false; bits[1] = false; bits[2] = true;  bits[3] = false; bits[4] = false; }
                if (code[0] == '1' && code[1] == '2' && code[2] == '3') { bits[0] = false; bits[1] = false; bits[2] = true;  bits[3] = false; bits[4] = true;  }
                if (code[0] == '1' && code[1] == '3' && code[2] == '1') { bits[0] = false; bits[1] = false; bits[2] = true;  bits[3] = true;  bits[4] = false; }
                if (code[0] == '1' && code[1] == '3' && code[2] == '2') { bits[0] = false; bits[1] = false; bits[2] = true;  bits[3] = true;  bits[4] = true;  }
                if (code[0] == '1' && code[1] == '3' && code[2] == '3') { bits[0] = false; bits[1] = true;  bits[2] = false; bits[3] = false; bits[4] = false; }
                if (code[0] == '2' && code[1] == '1' && code[2] == '1') { bits[0] = false; bits[1] = true;  bits[2] = false; bits[3] = false; bits[4] = true;  }
                if (code[0] == '2' && code[1] == '1' && code[2] == '2') { bits[0] = false; bits[1] = true;  bits[2] = false; bits[3] = true;  bits[4] = false; }
                if (code[0] == '2' && code[1] == '1' && code[2] == '3') { bits[0] = false; bits[1] = true;  bits[2] = false; bits[3] = true;  bits[4] = true;  }
                if (code[0] == '2' && code[1] == '2' && code[2] == '1') { bits[0] = false; bits[1] = true;  bits[2] = true;  bits[3] = false; bits[4] = false; }
                if (code[0] == '2' && code[1] == '2' && code[2] == '2') { bits[0] = false; bits[1] = true;  bits[2] = true;  bits[3] = false; bits[4] = true;  }
                if (code[0] == '2' && code[1] == '2' && code[2] == '3') { bits[0] = false; bits[1] = true;  bits[2] = true;  bits[3] = true;  bits[4] = false; }
                if (code[0] == '2' && code[1] == '3' && code[2] == '1') { bits[0] = false; bits[1] = true;  bits[2] = true;  bits[3] = true;  bits[4] = true;  }
                if (code[0] == '2' && code[1] == '3' && code[2] == '2') { bits[0] = true;  bits[1] = false; bits[2] = false; bits[3] = false; bits[4] = false; }
                if (code[0] == '2' && code[1] == '3' && code[2] == '3') { bits[0] = true;  bits[1] = false; bits[2] = false; bits[3] = false; bits[4] = true;  }
                if (code[0] == '3' && code[1] == '1' && code[2] == '1') { bits[0] = true;  bits[1] = false; bits[2] = false; bits[3] = true;  bits[4] = false; }
                if (code[0] == '3' && code[1] == '1' && code[2] == '2') { bits[0] = true;  bits[1] = false; bits[2] = false; bits[3] = true;  bits[4] = true;  }
                if (code[0] == '3' && code[1] == '1' && code[2] == '3') { bits[0] = true;  bits[1] = false; bits[2] = true;  bits[3] = false; bits[4] = false; }
                if (code[0] == '3' && code[1] == '2' && code[2] == '1') { bits[0] = true;  bits[1] = false; bits[2] = true;  bits[3] = false; bits[4] = true;  }
                if (code[0] == '3' && code[1] == '2' && code[2] == '2') { bits[0] = true;  bits[1] = false; bits[2] = true;  bits[3] = true;  bits[4] = false; }
                if (code[0] == '3' && code[1] == '2' && code[2] == '3') { bits[0] = true;  bits[1] = false; bits[2] = true;  bits[3] = true;  bits[4] = true;  }
                if (code[0] == '3' && code[1] == '3' && code[2] == '1') { bits[0] = true;  bits[1] = true;  bits[2] = false; bits[3] = false; bits[4] = false; }
                if (code[0] == '3' && code[1] == '3' && code[2] == '2') { bits[0] = true;  bits[1] = true;  bits[2] = false; bits[3] = false; bits[4] = true;  }
                if (code[0] == '3' && code[1] == '3' && code[2] == '3') { bits[0] = true;  bits[1] = true;  bits[2] = false; bits[3] = true;  bits[4] = false; }
                
                // Вторая группа
                if (code[3] == '1' && code[4] == '2' && code[5] == '3') { bits[5] = false; bits[6] = false; bits[7] = false; }
                if (code[3] == '1' && code[4] == '3' && code[5] == '2') { bits[5] = false; bits[6] = false; bits[7] = true;  }
                if (code[3] == '2' && code[4] == '1' && code[5] == '3') { bits[5] = false; bits[6] = true;  bits[7] = false; }
                if (code[3] == '2' && code[4] == '3' && code[5] == '1') { bits[5] = false; bits[6] = true;  bits[7] = true;  }
                if (code[3] == '3' && code[4] == '1' && code[5] == '2') { bits[5] = true;  bits[6] = false; bits[7] = false; }
                if (code[3] == '3' && code[4] == '2' && code[5] == '1') { bits[5] = true;  bits[6] = false; bits[7] = true;  }

                // Третья группа
                if (code[6] == '1' && code[7] == '2' && code[8] == '3') { bits[8] = false; bits[9] = false; bits[10] = false; }
                if (code[6] == '1' && code[7] == '3' && code[8] == '2') { bits[8] = false; bits[9] = false; bits[10] = true;  }
                if (code[6] == '2' && code[7] == '1' && code[8] == '3') { bits[8] = false; bits[9] = true;  bits[10] = false; }
                if (code[6] == '2' && code[7] == '3' && code[8] == '1') { bits[8] = false; bits[9] = true;  bits[10] = true;  }
                if (code[6] == '3' && code[7] == '1' && code[8] == '2') { bits[8] = true;  bits[9] = false; bits[10] = false; }
                if (code[6] == '3' && code[7] == '2' && code[8] == '1') { bits[8] = true;  bits[9] = false; bits[10] = true;  }

                // Четвертая группа
                if (code[9] == '1' && code[10] == '2' && code[11] == '3') { bits[11] = false; bits[12] = false; bits[13] = false; }
                if (code[9] == '1' && code[10] == '3' && code[11] == '2') { bits[11] = false; bits[12] = false; bits[13] = true;  }
                if (code[9] == '2' && code[10] == '1' && code[11] == '3') { bits[11] = false; bits[12] = true;  bits[13] = false; }
                if (code[9] == '2' && code[10] == '3' && code[11] == '1') { bits[11] = false; bits[12] = true;  bits[13] = true;  }
                if (code[9] == '3' && code[10] == '1' && code[11] == '2') { bits[11] = true;  bits[12] = false; bits[13] = false; }
                if (code[9] == '3' && code[10] == '2' && code[11] == '1') { bits[11] = true;  bits[12] = false; bits[13] = true;  }

                // Пятая группа
                if (code[12] == '1' && code[13] == '2' && code[14] == '3') { bits[14] = false; bits[15] = false; bits[16] = false; }
                if (code[12] == '1' && code[13] == '3' && code[14] == '2') { bits[14] = false; bits[15] = false; bits[16] = true;  }
                if (code[12] == '2' && code[13] == '1' && code[14] == '3') { bits[14] = false; bits[15] = true;  bits[16] = false; }
                if (code[12] == '2' && code[13] == '3' && code[14] == '1') { bits[14] = false; bits[15] = true;  bits[16] = true;  }
                if (code[12] == '3' && code[13] == '1' && code[14] == '2') { bits[14] = true;  bits[15] = false; bits[16] = false; }
                if (code[12] == '3' && code[13] == '2' && code[14] == '1') { bits[14] = true;  bits[15] = false; bits[16] = true;  }

                bits.CopyTo(intArray, 0);
                          
                codes.Add(intArray[0]);
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = codes[bar];
        }

        public static ThreeCandlesPatternCode Series(Bars bars)
        {
            string description = String.Format("ThreeCandlesPatternCode");
            if (bars.Cache.ContainsKey(description))
                return (ThreeCandlesPatternCode)bars.Cache[description];
            var threeCandlesPatternCode = new ThreeCandlesPatternCode(bars, description);
            bars.Cache[description] = threeCandlesPatternCode;
            return threeCandlesPatternCode;
        }

        /// <summary>
        /// Возвращает код паттерна
        /// </summary>
        private string GetPatternCode(List<double> openPrices,
                                      List<double> closePrices,
                                      List<double> highPrices,
                                      List<double> lowPrices,
                                      double minStepPrice)
        {            
            string group1 = "";

            // Указываем цвета свечей (1 - белая, 2 - черная, 3 - доджи)
            for (int i = 0; i < openPrices.Count; i++)
            {
                if (Math.Abs(closePrices[i] - openPrices[i]) < 3 * minStepPrice) group1 += "3";
                else if (closePrices[i] > openPrices[i]) group1 += "1";
                else if (closePrices[i] < openPrices[i]) group1 += "2";
            }

            // В порядке возрастания цен Open присваиваем номера
            string group2 = SortRange(openPrices);

            // В порядке возрастания цен Close присваиваем номера
            string group3 = SortRange(closePrices);

            // В порядке возрастания цен High присваиваем номера
            string group4 = SortRange(highPrices);

            // В порядке возрастания цен Low присваиваем номера
            string group5 = SortRange(lowPrices);

            string code = group1 + group2 + group3 + group4 + group5;

            return code;
        }

        /// <summary>
        /// Присвоение номеров в порядке возрастания
        /// </summary>
        private string SortRange(List<double> scores)
        {
            string result = String.Empty;

            var dict = scores.OrderByDescending(x => x)
                             .Distinct()
                             .Select((x, i) => new { Key = x, Value = i + 1 })
                             .ToDictionary(arg => arg.Key, arg => arg.Value);

            var ranges = scores.Select(x => new { Score = x, Place = dict[x] }).ToList();

            return ranges.Aggregate(result, (current, range) => current + range.Place);
        }
    }
}
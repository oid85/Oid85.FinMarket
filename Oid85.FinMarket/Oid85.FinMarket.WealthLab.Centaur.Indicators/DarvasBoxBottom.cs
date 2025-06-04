using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class DarvasBoxBottomHelper : IndicatorHelper
    {
        public override IList<object> ParameterDefaultValues { get { return new object[] {BarDataType.Bars}; } }
        public override IList<string> ParameterDescriptions { get { return new string[] {"Источник"}; } }
        public override string TargetPane { get { return "P"; } }
        public override Color DefaultColor { get { return Color.Blue; } }
        public override Type IndicatorType { get { return typeof(DarvasBoxBottom); } }
        public override string Description { get { return "DarvasBoxBottom"; } }
    }

    public class DarvasBoxBottom : DataSeries
    {
        public DarvasBoxBottom(Bars bars, string description)
            : base(bars, description)
        {
            FirstValidValue = 10;

            if (bars.Count < FirstValidValue)
                return;

            var darvasBoxTop = new DataSeries(bars.Close - bars.Close, @"darvasBoxTop");
            var darvasBoxBottom = new DataSeries(bars.Close - bars.Close, @"darvasBoxBottom");

            double boxTop = 0;
            double boxBottom = 0;
            int state = 1;

            for (int i = bars.Count - 1; i > 0; i--)
            {
                if (state == 1)
                {
                    boxTop = bars.High[i];
                }
                else if (state == 2)
                {
                    if (boxTop > bars.High[i])
                    {
                    }
                    else
                    {
                        boxTop = bars.High[i];
                    }
                }
                else if (state == 3)
                {
                    if (boxTop > bars.High[i])
                    {
                        boxBottom = bars.Low[i];
                    }
                    else
                    {
                        boxTop = bars.High[i];
                    }
                }
                else if (state == 4)
                {
                    if (boxTop > bars.High[i])
                    {
                        if (boxBottom < bars.Low[i])
                        {
                        }
                        else
                        {
                            boxBottom = bars.Low[i];
                        }
                    }
                    else
                    {
                        boxTop = bars.High[i];
                    }
                }
                else if (state == 5)
                {
                    if (boxTop > bars.High[i])
                    {
                        if (boxBottom < bars.Low[i])
                        {
                        }
                        else
                        {
                            boxBottom = bars.Low[i];
                        }
                    }
                    else
                    {
                        boxTop = bars.High[i];
                    }
                    state = 0;
                }
                darvasBoxTop[i] = boxTop;
                darvasBoxBottom[i] = boxBottom;
                state++;
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = darvasBoxBottom[bar];

        }

        /// <summary>
        /// Возвращает экзепляр индикатора
        /// </summary>
        /// <returns>Экземпляр индикатора: новый или из кеша</returns>
        public static DarvasBoxBottom Series(Bars bars)
        {
            const string description = "DarvasBoxBottom"; // Описание на графике
            if (bars.Cache.ContainsKey(description)) // Если индикатор есть в кеше
                return (DarvasBoxBottom)bars.Cache[description]; // то вернуть его из кеша
            var darvasBoxBottom = new DarvasBoxBottom(bars, description); // Создаем индикатор
            bars.Cache[description] = darvasBoxBottom; // Заносим его в кеш
            return darvasBoxBottom; // возвращаем его
        }
    }
}
using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class APTRHelper : IndicatorHelper
    {
        public override string Description { get {  return @"Средний процентный истинный диапазон от Vitali Apirine"; } }
        public override string URL { get {  return @"http://www2.wealth-lab.com/WL5Wiki/APTR.ashx"; } }
        public override Type IndicatorType { get {  return typeof(APTR); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары", "Период" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 300) }; } }
        public override string TargetPane { get { return "APTR"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.RoyalBlue; } }
    }

    public class APTR : DataSeries
    {
        public APTR(Bars Bars, int Period, string Description)
            : base(Bars, Description)
        {
            FirstValidValue = Period + 1;
            var highLow = Bars.High - Bars.Low; // Current high – Current low
            var highPrevClose = Abs(Bars.High - (Bars.Close >> 1)); // Current high – Previous close
            var lowPrevClose = Abs(Bars.Low - (Bars.Close >> 1)); // Current low – Previous close
            var ptr1 = highLow / (highLow / 2d + Bars.Low); // (Current high – Current low) / (((Current high – Current low)/2) + Current low)
            var ptr2 = highPrevClose / (highPrevClose / 2d + (Bars.Close >> 1)); // (Current high – Previous close) / (((Current high – Previous close)/2) + Previous close)
            var ptr3 = lowPrevClose / (lowPrevClose / 2d + Bars.Low); // (Current low – Previous close) / (((Current low – Previous close)/2) + Current low)

            for (int bar = 1; bar < Bars.Count; bar++)
            {
                double ptr = Math.Max(ptr1[bar], Math.Max(ptr2[bar], ptr3[bar])); // Максимальное значение из 3-х кандидатов               
                this[bar] = (this[bar - 1] * (Period - 1) + ptr) / Period; // Current APTR = [(Prior APTR x 13) + Current PTR]/14              
            }
        }

        public static APTR Series(Bars Bars, int Period)
        {
            string description = String.Format("APTR({0})", Period); // Описание на графике
            if (Bars.Cache.ContainsKey(description)) // Если индикатор есть в кеше
                return (APTR)Bars.Cache[description]; // то вернуть его из кеша
            var result = new APTR(Bars, Period, description); // Иначе создаем индикатор
            Bars.Cache[description] = result; // Заносим его в кеш
            return result; // Возвращаем его
        }
    }
}
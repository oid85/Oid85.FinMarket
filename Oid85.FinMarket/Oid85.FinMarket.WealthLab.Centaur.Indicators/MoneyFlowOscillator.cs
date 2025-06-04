using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class MoneyFlowOscillatorHelper : IndicatorHelper
    {
        public override string Description { get {  return @"Осциллятор денежного потока от Vitali Apirine"; } }
        public override string URL { get { return @"http://www2.wealth-lab.com/WL5Wiki/MoneyFlowOscillator.ashx"; } }
        public override Type IndicatorType { get { return typeof(MoneyFlowOscillator); } }
        public override IList<string> ParameterDescriptions { get { return new[] { "Бары", "Период" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { BarDataType.Bars, new RangeBoundInt32(20, 2, 300) }; } }
        public override string TargetPane { get { return "MoneyFlowOscillator"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Black; } }
        public override bool IsOscillator { get { return true; } }
    }

    public class MoneyFlowOscillator : DataSeries
    {
        public MoneyFlowOscillator(Bars Bars, int Period, string Description)
            : base(Bars, Description)
        {
            FirstValidValue = Period;
            DataSeries multiplier = (Bars.High - (Bars.Low >> 1) - ((Bars.High >> 1) - Bars.Low)) / // [(High – previous low) – (Previous high – low)] /
                (Bars.High - (Bars.Low >> 1) + ((Bars.High >> 1) - Bars.Low)); // [(High – previous low) + (Previous high – low)]
            DataSeries mfv = multiplier * Bars.Volume; // Money fow volume = Multiplier * volume for the period
            DataSeries mfo = Sum.Series(mfv, Period) / Sum.Series(Bars.Volume, Period); // 20-period MFO = 20-period sum of money flow volume / 20-period sum of volume

            for (int bar = FirstValidValue; bar < Bars.Count; bar++)
                this[bar] = mfo[bar];
        }

        public static MoneyFlowOscillator Series(Bars Bars, int Period)
        {            
            string description = String.Format("MoneyFlowOscillator({0})", Period); // Описание на графике
            if (Bars.Cache.ContainsKey(description)) // Если индикатор есть в кеше
                return (MoneyFlowOscillator)Bars.Cache[description]; // то вернуть его из кеша
            var result = new MoneyFlowOscillator(Bars, Period, description); // Иначе создаем индикатор
            Bars.Cache[description] = result; // Заносим его в кеш
            return result; // Возвращаем его
        }
    }
}
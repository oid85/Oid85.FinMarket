using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class ModifiedATRHelper : IndicatorHelper
    {
        public override string Description { get { return @"Модифицированный средний истинный диапазон"; } }
        public override Type IndicatorType { get { return typeof(ModifiedATR); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары", "Период" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars, new RangeBoundInt32(20, 1, 100) }; } }
        public override string TargetPane { get { return "ModifiedATR"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Red; } }
    }

    public class ModifiedATR : DataSeries
    {
        public ModifiedATR(Bars bars, int period, string description)
            : base(bars, description)
        {
            FirstValidValue = period + 1;

            DataSeries modifiedTR = ModifiedTR.Series(bars);

            for (int bar = 1; bar < bars.Count; bar++)
                this[bar] = Math.Round(SMA.Series(modifiedTR, period)[bar], 2);
        }

        public static ModifiedATR Series(Bars bars, int period)
        {
            string description = String.Format("ModifiedATR({0})", period); // Описание на графике
            if (bars.Cache.ContainsKey(description)) // Если индикатор есть в кеше
                return (ModifiedATR)bars.Cache[description]; // то вернуть его из кеша
            var result = new ModifiedATR(bars, period, description); // Иначе создаем индикатор
            bars.Cache[description] = result; // Заносим его в кеш
            return result; // Возвращаем его
        }
    }
}
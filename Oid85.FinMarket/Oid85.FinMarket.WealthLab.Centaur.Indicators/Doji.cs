using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class DojiHelper : IndicatorHelper
    {
        public override string Description { get { return @"Doji"; } }
        public override Type IndicatorType { get { return typeof(Doji); } }
        public override IList<string> ParameterDescriptions { get {  return new[] { "Бары" }; } }
        public override IList<object> ParameterDefaultValues { get {  return new object[] { BarDataType.Bars }; } }
        public override string TargetPane { get { return "Doji"; } }
        public override LineStyle DefaultStyle { get {  return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.DarkBlue; } }        
    }

    public class Doji : DataSeries
    {
        public Doji(Bars bars, string description)
            : base(bars, description)
        {
            var doji = new DataSeries(bars.Close - bars.Close, @"doji");

            for (int bar = 1; bar < bars.Count; bar++)
            {
                if (Math.Abs(bars.Open[bar] - bars.Close[bar]) / bars.SymbolInfo.Tick < 6.0)
                {
                    doji[bar] = 1.0;
                }
            }

            for (int bar = 0; bar < bars.Count; bar++)
                this[bar] = doji[bar];
        }

        public static Doji Series(Bars bars)
        {
            string description = String.Format("Doji)");
            if (bars.Cache.ContainsKey(description))
                return (Doji)bars.Cache[description];
            var result = new Doji(bars, description);
            bars.Cache[description] = result;
            return result;
        }
    }
}
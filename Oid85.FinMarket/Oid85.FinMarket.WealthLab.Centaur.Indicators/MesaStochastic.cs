using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class MesaStochasticHelper : IndicatorHelper
    {
        public override string Description { get { return @"MESA Stochastic �� John Ehlers"; } }
        public override string URL { get { return @"http://www2.wealth-lab.com/WL5WIKI/TASCJan2014.ashx"; } }
        public override Type IndicatorType { get { return typeof(MesaStochastic); } }
        public override IList<string> ParameterDescriptions { get { return new[] { "��������", "������" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 2, 100) }; } }
        public override string TargetPane { get { return "MesaStochastic"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Red; } }
        public override bool IsOscillator { get { return true; } }
        public override double OscillatorOverboughtValue { get { return 80; } }
        public override double OscillatorOversoldValue { get { return 20; } }
    }

    /// <summary>
    /// MESA Stochastic
    /// </summary>
    public class MesaStochastic : DataSeries
    {
        public MesaStochastic(DataSeries DS, int Period, string Description)
            : base(DS, Description)
        {
            const int roofingPeriod = 48; // ������ ������� Roofing
            var roofing = Roofing.Series(DS, roofingPeriod); // ������� ����������� ����� Roofing(48) � SuperSmoother(10)

            var maxRoothing = Highest.Series(roofing, Period); // ���������� �������� Roofing �� ������
            var minRoothing = Lowest.Series(roofing, Period); // ���������� �������� Roofing �� ������
            var stoc = (roofing - minRoothing) / (maxRoothing - minRoothing); // ������� ��������� �� Roofing

            const int ssPeriod = 10; // ������ ������� SuperSmoother
            var mesaStoch = SuperSmoother.Series(stoc, ssPeriod); // ������ ����������� ����� SuperSmoother(10)
            FirstValidValue = Period + 2;

            for (int bar = FirstValidValue; bar < DS.Count; bar++) // ����������� �� ���� �����
                this[bar] = mesaStoch[bar] * 100;
        }

        public static MesaStochastic Series(DataSeries DS, int Period)
        {
            string description = String.Format("MesaStochastic({0}, {1})", DS.Description, Period); // �������� �� �������
            if (DS.Cache.ContainsKey(description)) // ���� ��������� ���� � ����
                return (MesaStochastic)DS.Cache[description]; // �� ������� ��� �� ����
            var result = new MesaStochastic(DS, Period, description); // ����� ������� ���������
            DS.Cache[description] = result; // ������� ��� � ���
            return result; // ���������� ���
        }
    }
}
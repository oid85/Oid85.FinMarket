using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class MesaOscillatorHelper : IndicatorHelper
    {
        public override string Description { get { return @"MESA Oscillator �� John Ehlers"; } }
        public override string URL { get { return @"http://www2.wealth-lab.com/WL5WIKI/TASCJan2015.ashx"; } }
        public override Type IndicatorType { get { return typeof(MesaOscillator); } }
        public override IList<string> ParameterDescriptions { get { return new[] { "��������", "�������" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 1, 200) }; } }
        public override string TargetPane { get { return "MesaOscillator"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Red; } }
    }

    /// <summary>
    /// MESA Oscillator
    /// </summary>
    public class MesaOscillator : DataSeries
    {
        public MesaOscillator(DataSeries DS, int BandEdge, string Description)
            : base(DS, Description)
        {
            var whiteNoise = (DS - (DS >> 2)) / 2d; // ����� ���. ������� �������� �� 2-� ��������� ����� (������� ���������)
            var superSmoother = SuperSmoother.Series(whiteNoise, BandEdge); // ����������� �������� SuperSmoother
            double peakAutoGainControl = 0.0000001;
            FirstValidValue = 2;
            this[0] = 0;
            this[1] = 0;

            for (int bar = FirstValidValue; bar < DS.Count; bar++) // ����������� �� ���� �����
            {
                peakAutoGainControl *= 0.991; // ������������� �������� �������
                peakAutoGainControl = Math.Max(peakAutoGainControl, Math.Abs(superSmoother[bar])); // ���� ��������, ��������� �������
                this[bar] = superSmoother[bar] / peakAutoGainControl;
            }

        }

        public static MesaOscillator Series(DataSeries DS, int BandEdge)
        {
            string description = String.Format("MesaOscillator({0}, {1})", DS.Description, BandEdge); // �������� �� �������
            if (DS.Cache.ContainsKey(description)) // ���� ��������� ���� � ����
                return (MesaOscillator)DS.Cache[description]; // �� ������� ��� �� ����
            var result = new MesaOscillator(DS, BandEdge, description); // ����� ������� ���������
            DS.Cache[description] = result; // ������� ��� � ���
            return result; // ���������� ���
        }
    }
}
using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class RoofingHelper : IndicatorHelper
    {
        public override string Description { get { return @"������ Roofing �� John Ehlers"; } }
        public override string URL { get { return @"http://www2.wealth-lab.com/WL5WIKI/TASCJan2014.ashx"; } }
        public override Type IndicatorType { get { return typeof(Roofing); } }
        public override IList<string> ParameterDescriptions { get { return new[] { "��������", "������" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { CoreDataSeries.Close, new RangeBoundInt32(10, 1, 200) }; } }
        public override string TargetPane { get { return "Roofing"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Red; } }
    }

    /// <summary>
    /// ������ Roofing
    /// </summary>
    public class Roofing : DataSeries
    {
        public Roofing(DataSeries DS, int Period, string Description)
            : base(DS, Description)
        {
            const int ssPeriod = 10; // ������ ������� SuperSmoother
            FirstValidValue = 15; // � SS � HPS ���������� �� 2-�� ����, �� ��� ��� ����� ������������, �.�. ������������ �������� �����
            var highPassFilter = HighpassFilter(DS, Period); // ������ ������� ������
            var superSmoother = SuperSmoother.Series(highPassFilter, ssPeriod);
            for (int bar = FirstValidValue; bar < DS.Count; bar++) // ����������� �� ���� �����
                this[bar] = superSmoother[bar];
        }

        public static Roofing Series(DataSeries DS, int Period)
        {
            string description = String.Format("Roofing({0}, {1})", DS.Description, Period); // �������� �� �������
            if (DS.Cache.ContainsKey(description)) // ���� ��������� ���� � ����
                return (Roofing)DS.Cache[description]; // �� ������� ��� �� ����
            var result = new Roofing(DS, Period, description); // ����� ������� ���������
            DS.Cache[description] = result; // ������� ��� � ���
            return result; // ���������� ���
        }

        public static DataSeries HighpassFilter(DataSeries DS, int Period)
        {
            double sqrt2 = Math.Sqrt(2); // ���������� ������ �� 2
            double cosInDegrees = Math.Cos(sqrt2 * Math.PI / Period);
            double sinInDegrees = Math.Sin(sqrt2 * Math.PI / Period);
            double alpha1 = (cosInDegrees + sinInDegrees - 1) / cosInDegrees; // Highpass filter cyclic components whose periods are shorter than 48 bars

            var highPassFilter = new DataSeries(DS, "Highpass Filter") // ������ ������� ������
            {
                FirstValidValue = 2 // ���������, ���������� ����� ������� �� 3-�� ����, �� ����� ������������                                
            };

            highPassFilter[0] = 0; // ������ � ������ ��������
            highPassFilter[1] = 0;  // ����� ��������

            for (int bar = highPassFilter.FirstValidValue; bar < DS.Count; bar++) // ����������� �� ���� �����
                highPassFilter[bar] =
                    Math.Pow(1d - alpha1 / 2d, 2) * (DS[bar] - 2 * DS[bar - 1] + DS[bar - 2]) +
                    2 * (1d - alpha1) * highPassFilter[bar - 1] -
                    Math.Pow(1d - alpha1, 2) * highPassFilter[bar - 2]; // ������� ������� ������� ������
            return highPassFilter;
        }
    }
}
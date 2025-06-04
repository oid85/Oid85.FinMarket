using System.Drawing;
using WealthLab;

namespace Oid85.FinMarket.WealthLab.Centaur.Indicators
{
    public class SuperSmootherHelper : IndicatorHelper
    {
        public override string Description { get { return @"������ SuperSmoother �� John Ehlers"; } }
        public override string URL { get { return @"http://www2.wealth-lab.com/WL5WIKI/TASCJan2014.ashx"; } }
        public override Type IndicatorType { get { return typeof(SuperSmoother); } }
        public override IList<string> ParameterDescriptions { get { return new[] { "��������", "������" }; } }
        public override IList<object> ParameterDefaultValues { get { return new object[] { CoreDataSeries.Close, new RangeBoundInt32(10, 1, 200) }; } }
        public override string TargetPane { get { return "P"; } }
        public override LineStyle DefaultStyle { get { return LineStyle.Solid; } }
        public override Color DefaultColor { get { return Color.Red; } }
    }

    /// <summary>
    /// ������ SuperSmoother
    /// </summary>
    public class SuperSmoother : DataSeries
    {
        public SuperSmoother(DataSeries DS, int Period, string Description)
            : base(DS, Description)
        {
            double sqrt2 = Math.Sqrt(2); // ���������� ������ �� 2
            double a1 = Math.Exp(-sqrt2 * Math.PI / Period);
            double b1 = 2d * a1 * Math.Cos(sqrt2 * Math.PI / Period);
            double c2 = b1;
            double c3 = -a1 * a1;
            double c1 = 1 - c2 - c3;

            FirstValidValue = 2;
            this[0] = 0;
            this[1] = 0;

            for (int bar = FirstValidValue; bar < DS.Count; bar++) // ����������� �� ���� �����
                this[bar] = c1 * (DS[bar] + DS[bar - 1]) / 2d + c2 * this[bar - 1] + c3 * this[bar - 2];
        }

        public static SuperSmoother Series(DataSeries DS, int Period)
        {
            string description = String.Format("SuperSmoother({0}, {1})", DS.Description, Period); // �������� �� �������
            if (DS.Cache.ContainsKey(description)) // ���� ��������� ���� � ����
                return (SuperSmoother)DS.Cache[description]; // �� ������� ��� �� ����
            var result = new SuperSmoother(DS, Period, description); // ����� ������� ���������
            DS.Cache[description] = result; // ������� ��� � ���
            return result; // ���������� ���
        }
    }
}
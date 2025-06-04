using WealthLab;

namespace Oid85.FinMarket.Strategies.Indicators
{
    public interface IDataSeriesIndicator
    {
        DataSeries Init(List<double> source);
    }
}

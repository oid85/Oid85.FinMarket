namespace Oid85.FinMarket.Strategies.Indicators
{
    public interface IIndicator : IBarIndicator, IDataSeriesIndicator
    {
        List<double> Values { get; set; }
    }
}

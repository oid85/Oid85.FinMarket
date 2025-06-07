namespace Oid85.FinMarket.Application.Interfaces.Factories;

public interface IIndicatorFactory
{
    List<double> Highest(List<double> values, int period);
    List<double> Lowest(List<double> values, int period);
    List<double> Sma(List<double> values, int period);
}
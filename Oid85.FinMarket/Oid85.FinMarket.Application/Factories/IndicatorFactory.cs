using Oid85.FinMarket.Application.Interfaces.Factories;

namespace Oid85.FinMarket.Application.Factories;

public class IndicatorFactory : IIndicatorFactory
{
    public List<double> Highest(List<double> values, int period) => 
        values.Select((_, i) => i - period < 0 ? 0.0 : values.Skip(i - period + 1).Take(period).Max()).ToList();

    public List<double> Lowest(List<double> values, int period) => 
        values.Select((_, i) => i - period < 0 ? 0.0 : values.Skip(i - period + 1).Take(period).Min()).ToList();

    public List<double> Sma(List<double> values, int period) => 
        values.Select((_, i) => i - period < 0 ? 0.0 : values.Skip(i - period + 1).Take(period).Average()).ToList();
}
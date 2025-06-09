using Oid85.FinMarket.Application.Factories;
using Oid85.FinMarket.Application.Interfaces.Factories;

namespace Test.Oid85.FinMarket.Application;

public class IndicatorFactoryTests
{
    private readonly IIndicatorFactory _indicatorFactory;
    
    public IndicatorFactoryTests()
    {
        _indicatorFactory = new IndicatorFactory();
    }
    
    [Fact]
    public void Highest_return_is_correct()
    {
        // Arrange
        const int period = 3;
        var values = new List<double> {0.1, 0.2, 0.3, 0.2, 0.5, 0.6, 0.5};
        var etalon = new List<double> {0.0, 0.0, 0.0, 0.3, 0.5, 0.6, 0.6};
        
        // Act
        var indicatorValues = _indicatorFactory.Highest(values, period);
        
        // Assert
        Assert.True(Math.Abs(indicatorValues[0] - etalon[0]) < 0.01);
        Assert.True(Math.Abs(indicatorValues[1] - etalon[1]) < 0.01);
        Assert.True(Math.Abs(indicatorValues[2] - etalon[2]) < 0.01);
        Assert.True(Math.Abs(indicatorValues[3] - etalon[3]) < 0.01);
        Assert.True(Math.Abs(indicatorValues[4] - etalon[4]) < 0.01);
        Assert.True(Math.Abs(indicatorValues[5] - etalon[5]) < 0.01);
        Assert.True(Math.Abs(indicatorValues[6] - etalon[6]) < 0.01);
    }
    
    [Fact]
    public void Lowest_return_is_correct()
    {
        // Arrange
        const int period = 3;
        var values = new List<double> {0.1, 0.2, 0.3, 0.2, 0.5, 0.6, 0.5};
        var etalon = new List<double> {0.0, 0.0, 0.0, 0.2, 0.2, 0.2, 0.5};
        
        // Act
        var indicatorValues = _indicatorFactory.Lowest(values, period);
        
        // Assert
        Assert.True(Math.Abs(indicatorValues[0] - etalon[0]) < 0.01);
        Assert.True(Math.Abs(indicatorValues[1] - etalon[1]) < 0.01);
        Assert.True(Math.Abs(indicatorValues[2] - etalon[2]) < 0.01);
        Assert.True(Math.Abs(indicatorValues[3] - etalon[3]) < 0.01);
        Assert.True(Math.Abs(indicatorValues[4] - etalon[4]) < 0.01);
        Assert.True(Math.Abs(indicatorValues[5] - etalon[5]) < 0.01);
        Assert.True(Math.Abs(indicatorValues[6] - etalon[6]) < 0.01);
    }  
    
    [Fact]
    public void Sma_return_is_correct()
    {
        // Arrange
        const int period = 3;
        var values = new List<double> {0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7};
        var etalon = new List<double> {0.0, 0.0, 0.0, 0.3, 0.4, 0.5, 0.6};
        
        // Act
        var indicatorValues = _indicatorFactory.Sma(values, period);
        
        // Assert
        Assert.True(Math.Abs(indicatorValues[0] - etalon[0]) < 0.01);
        Assert.True(Math.Abs(indicatorValues[1] - etalon[1]) < 0.01);
        Assert.True(Math.Abs(indicatorValues[2] - etalon[2]) < 0.01);
        Assert.True(Math.Abs(indicatorValues[3] - etalon[3]) < 0.01);
        Assert.True(Math.Abs(indicatorValues[4] - etalon[4]) < 0.01);
        Assert.True(Math.Abs(indicatorValues[5] - etalon[5]) < 0.01);
        Assert.True(Math.Abs(indicatorValues[6] - etalon[6]) < 0.01);
    }     
}
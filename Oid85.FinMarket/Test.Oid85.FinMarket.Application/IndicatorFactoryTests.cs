using Oid85.FinMarket.Application.Factories;
using Oid85.FinMarket.Application.Interfaces.Factories;

namespace Test.Oid85.FinMarket.Application;

public class IndicatorFactoryTests
{
    private IIndicatorFactory _indicatorFactory;
    
    public IndicatorFactoryTests()
    {
        _indicatorFactory = new IndicatorFactory();
    }
    
    [Fact]
    public void Highest_return_is_correct()
    {
        // Arrange
        var period = 3;
        var values = new List<double>() {0.1, 0.2, 0.3, 0.2, 0.5, 0.6, 0.5};
        var expectedValues = new List<double>() {0.0, 0.0, 0.0, 0.3, 0.5, 0.6, 0.6};
        
        // Act
        var indicatorValues = _indicatorFactory.Highest(values, period);
        
        // Assert
        Assert.True(Math.Abs(indicatorValues[0] - expectedValues[0]) < 0.0001);
        Assert.True(Math.Abs(indicatorValues[1] - expectedValues[1]) < 0.0001);
        Assert.True(Math.Abs(indicatorValues[2] - expectedValues[2]) < 0.0001);
        Assert.True(Math.Abs(indicatorValues[3] - expectedValues[3]) < 0.0001);
        Assert.True(Math.Abs(indicatorValues[4] - expectedValues[4]) < 0.0001);
        Assert.True(Math.Abs(indicatorValues[5] - expectedValues[5]) < 0.0001);
        Assert.True(Math.Abs(indicatorValues[6] - expectedValues[6]) < 0.0001);
    }
}
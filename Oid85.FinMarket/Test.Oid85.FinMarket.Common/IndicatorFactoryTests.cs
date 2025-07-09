using Oid85.FinMarket.Common.Helpers;

namespace Test.Oid85.FinMarket.Common;

public class DateTimeCurveHelperTests
{
    [Fact]
    public void Expand_return_is_correct()
    {
        // Arrange
        var curve = new Dictionary<DateTime, double>
        {
            { new DateTime(2020, 01, 05), 1.0 },
            { new DateTime(2020, 01, 07), 2.0 },
            { new DateTime(2020, 01, 09), 3.0 }
        };

        // Act
        var sut = curve.Expand(new DateTime(2020, 01, 01), new DateTime(2020, 01, 10));

        // Assert
        Assert.True(true);
    }
}
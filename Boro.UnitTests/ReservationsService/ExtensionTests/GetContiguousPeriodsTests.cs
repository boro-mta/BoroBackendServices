using ReservationsService.DB.Extensions;

namespace Boro.UnitTests.ReservationsService.ExtensionTests;

[TestClass]
public class GetContiguousPeriodsTests
{
    [TestMethod]
    public void GetContiguousPeriods_ReturnsEmpty_WhenInputIsEmpty()
    {
        // Arrange
        var dates = Enumerable.Empty<DateTime>();

        // Act
        var result = dates.GetContiguousPeriods();

        // Assert
        var empty = Enumerable.Empty<(DateTime, DateTime)>();
        CollectionAssert.AreEqual(empty.ToList(), result.ToList());
    }

    [TestMethod]
    public void GetContiguousPeriods_ReturnsSinglePeriod_WhenInputContainsOnlyOneDate()
    {
        // Arrange
        var dates = new[] { new DateTime(2023, 4, 29) };

        // Act
        var result = dates.GetContiguousPeriods().ToList();

        // Assert
        Assert.AreEqual(1, result.Count);
        CollectionAssert.Contains(result, (new DateTime(2023, 4, 29), new DateTime(2023, 4, 29)));
    }

    [TestMethod]
    public void GetContiguousPeriods_ReturnsContiguousPeriods_WhenInputContainsMultipleDates()
    {
        // Arrange
        var dates = new[]
        {
            new DateTime(2023, 4, 1),
            new DateTime(2023, 4, 2),
            new DateTime(2023, 4, 4),
            new DateTime(2023, 4, 6),
            new DateTime(2023, 4, 7),
            new DateTime(2023, 4, 10),
            new DateTime(2023, 4, 11)
        };

        // Act
        var result = dates.GetContiguousPeriods().ToList();

        // Assert
        Assert.AreEqual(4, result.Count);
        CollectionAssert.Contains(result, (new DateTime(2023, 4, 1), new DateTime(2023, 4, 2)));
        CollectionAssert.Contains(result, (new DateTime(2023, 4, 4), new DateTime(2023, 4, 4)));
        CollectionAssert.Contains(result, (new DateTime(2023, 4, 6), new DateTime(2023, 4, 7)));
        CollectionAssert.Contains(result, (new DateTime(2023, 4, 10), new DateTime(2023, 4, 11)));
    }

    [TestMethod]
    public void GetContiguousPeriods_ReturnsContiguousPeriods_WhenInputContainsDuplicates()
    {
        // Arrange
        var dates = new[]
        {
            new DateTime(2023, 4, 1, 12, 15, 0),
            new DateTime(2023, 4, 2, 8, 5, 30),
            new DateTime(2023, 4, 2, 17, 20, 10),
            new DateTime(2023, 4, 4, 2, 45, 45),
            new DateTime(2023, 4, 4, 15, 35, 20),
            new DateTime(2023, 4, 5, 11, 25, 5),
            new DateTime(2023, 4, 6, 19, 50, 40),
            new DateTime(2023, 4, 6, 7, 30, 15),
            new DateTime(2023, 4, 7, 14, 10, 30)
        };

        // Act
        var result = dates.GetContiguousPeriods().ToList();

        // Assert
        Assert.AreEqual(2, result.Count);
        CollectionAssert.Contains(result, (new DateTime(2023, 4, 1), new DateTime(2023, 4, 2)));
        CollectionAssert.Contains(result, (new DateTime(2023, 4, 4), new DateTime(2023, 4, 7)));
    }

    [TestMethod]
    public void MultipleAdjacentDates_ReturnOnePeriod()
    {
        var dates = new List<DateTime>
        {
            new DateTime(2023, 5, 5, 1, 30, 0),
            new DateTime(2023, 5, 6, 2, 0, 0),
            new DateTime(2023, 5, 7, 3, 30, 0),
        };
        var periods = dates.GetContiguousPeriods().DateParts().ToList();
        Assert.AreEqual(1, periods.Count);
        CollectionAssert.Contains(periods, (new DateTime(2023, 5, 5), new DateTime(2023, 5, 7)));
    }
}

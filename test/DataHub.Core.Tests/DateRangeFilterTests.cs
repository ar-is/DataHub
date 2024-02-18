using System.Runtime.CompilerServices;
using DataHub.Core.Filters;
using DataHub.Core.Tests.TestData;

namespace DataHub.Core.Tests;

public class DateRangeFilterTests
{
    [TestCaseSource(typeof(DateRangeFiltersTestData), nameof(DateRangeFiltersTestData.ValidDates))]
    public void DateRangeFilter_Constructor_Parses_Valid_Dates(
        string dateFrom, 
        string dateTo, 
        DateOnly expectedDateFrom,
        DateOnly expectedDateTo)
    {
        // Arrange - Act
        var filter = new DateRangeFilter(dateFrom, dateTo);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(filter.DateFrom.HasValue, Is.EqualTo(true));
            Assert.That(filter.DateFrom, Is.EqualTo(expectedDateFrom));
            Assert.That(filter.DateTo.HasValue, Is.EqualTo(true));
            Assert.That(filter.DateTo, Is.EqualTo(expectedDateTo));
        });
    }

    [TestCaseSource(typeof(DateRangeFiltersTestData), nameof(DateRangeFiltersTestData.InvalidDates))]
    public void DateRangeFilter_Constructor_Uses_Default_Date_For_Invalid_Input(string dateFrom, string dateTo)
    {
        // Arrange - Act
        [UnsafeAccessor(UnsafeAccessorKind.StaticField, Name = "DefaultDate")]
        static extern ref DateOnly DateRangeFilterDefaultDate(DateRangeFilter dateRangeFilter);

        var filter = new DateRangeFilter(dateFrom, dateTo);
        var expectedDay = DateRangeFilterDefaultDate(filter);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(filter.DateFrom.HasValue, Is.EqualTo(true));
            Assert.That(filter.DateFrom?.Day, Is.EqualTo(expectedDay.Day));
            Assert.That(filter.DateTo.HasValue, Is.EqualTo(true));
            Assert.That(filter.DateTo?.Day, Is.EqualTo(expectedDay.Day));
        });
    }
}
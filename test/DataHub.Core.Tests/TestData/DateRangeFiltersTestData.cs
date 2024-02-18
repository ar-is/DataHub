using System.Collections;

namespace DataHub.Core.Tests.TestData;

internal static class DateRangeFiltersTestData
{
    internal static IEnumerable ValidDates()
    {
        yield return new TestCaseData("2024-01-01", "2024-01-10", new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
    }

    internal static IEnumerable InvalidDates()
    {
        yield return new TestCaseData("invalid_date", "invalid_date");
    }
}
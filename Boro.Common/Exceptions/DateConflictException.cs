namespace Boro.Common.Exceptions;

public class DateConflictException : Exception
{
    public DateConflictException(DateTime startDate, DateTime endDate)
        : base($"date range: {startDate} - {endDate} is conflicted")
    {
    }
}

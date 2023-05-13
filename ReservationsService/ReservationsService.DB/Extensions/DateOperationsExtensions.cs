using Microsoft.IdentityModel.Tokens;
using ReservationsService.API.Models;

namespace ReservationsService.DB.Extensions;

public static class DateOperationsExtensions
{
    public static IEnumerable<ReservationPeriod> DateParts(this IEnumerable<ReservationPeriod> pairs)
    {
        return pairs.Select(pair => new ReservationPeriod(pair.StartDate.Date, pair.EndDate.Date));
    }

    public static IEnumerable<ReservationPeriod> GetContiguousPeriods(this IEnumerable<DateTime> dates)
    {
        var results = Enumerable.Empty<ReservationPeriod>();

        if (dates.IsNullOrEmpty())
        {
            return results;
        }
        var ordered = dates.Order().Distinct().ToList();
        var startDate = ordered.First();
        var endDate = startDate;
        foreach (var date in ordered)
        {
            if ((date.Date - endDate.Date).TotalDays <= 1)
            {
                endDate = date;
            }
            else
            {
                results = results.Append(new ReservationPeriod(startDate, endDate));
                startDate = endDate = date;
            }
        }
        results = results.Append(new ReservationPeriod(startDate, endDate));
        return results;
    }

    public static IEnumerable<DateTime> Dates(this ReservationPeriod period)
    {
        var dates = Enumerable.Empty<DateTime>();
        for (DateTime date = period.StartDate; date <= period.EndDate; date = date.AddDays(1))
        {
            dates = dates.Append(date);
        }
        return dates;
    }
}

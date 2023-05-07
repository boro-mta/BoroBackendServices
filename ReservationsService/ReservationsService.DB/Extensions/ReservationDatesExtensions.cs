using Boro.EntityFramework.DbContexts.BoroMainDb.Enum;
using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using Microsoft.IdentityModel.Tokens;
using ReservationsService.API.Models.Input;
using ReservationsService.API.Models.Output;

namespace ReservationsService.DB.Extensions;

public static class ReservationDatesExtensions
{
    internal static bool InBound(this DateTime date, DateTime lowerBound,  DateTime upperBound)
    {
        return lowerBound <= date && date <= upperBound;
    }

    public static IEnumerable<DateTime> DateParts(this IEnumerable<DateTime> dates)
    {
        return dates.Select(d => d.Date);
    }

    public static IEnumerable<(DateTime StartDate, DateTime EndDate)> DateParts(this IEnumerable<(DateTime StartDate, DateTime EndDate)> pairs)
    {
        return pairs.Select(pair => (pair.StartDate.Date, pair.EndDate.Date));
    }

    public static IEnumerable<(DateTime StartDate, DateTime EndDate)> GetContiguousPeriods(this IEnumerable<DateTime> dates)
    {
        var results = Enumerable.Empty<(DateTime StartDate, DateTime EndDate)>();

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
                results = results.Append((startDate, endDate));
                startDate = endDate = date;
            }
        }
        results = results.Append((startDate, endDate));
        return results;
    }

    public static IEnumerable<DateTime> GetHoles(this IEnumerable<DateTime> dates)
    {
        var results = Enumerable.Empty<DateTime>();

        if (dates.IsNullOrEmpty())
        {
            return results;
        }

        var ordered = dates.Order().ToList();
        var first = ordered.First();
        foreach (var date in ordered.Skip(1))
        {
            var currentDate = first.AddDays(1);
            while (currentDate.Date < date.Date)
            {
                results = results.Append(currentDate);
                currentDate = currentDate.AddDays(1);
            }
            first = date;
        }

        return results;

    }

    internal static ReservedDates ToReservedDates(this Reservations entry)
    {
        return new ReservedDates
        {
            StartDate = entry.StartDate,
            EndDate = entry.EndDate,
            IsBlockedByOwner = false,
            Status = entry.Status,
        };
    }

    internal static Reservations ToTableEntry(this ReservedDates reservationDates, Guid itemId)
    {
        return new Reservations
        {
            ItemId = itemId,
            StartDate = reservationDates.StartDate,
            EndDate = reservationDates.EndDate,
        };
    }

    internal static Reservations ToTableEntry(this ReservationRequestInput reservationRequestInput, Guid reservationID, Guid itemId, Guid lenderId)
    {
        return new Reservations
        {
            ReservationId = reservationID,
            ItemId = itemId,
            StartDate = reservationRequestInput.StartDate,
            EndDate = reservationRequestInput.EndDate,
            Status = ReservationStatus.Pending,
            BorrowerId = Guid.Parse(reservationRequestInput.BorrowerId),
            LenderId = lenderId,
        };
    }

    internal static ReservationDetails ToReservationDetails(this Reservations entry)
    {
        return new ReservationDetails
        {
            ReservationId = entry.ReservationId,
            StartDate = entry.StartDate,
            EndDate = entry.EndDate,
            ItemId = entry.ItemId,
            BorrowerId = entry.BorrowerId,
            LenderId = entry.LenderId,
            Status = entry.Status
        };
    }
}
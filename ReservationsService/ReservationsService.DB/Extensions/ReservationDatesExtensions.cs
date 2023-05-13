using Boro.EntityFramework.DbContexts.BoroMainDb.Enum;
using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using Microsoft.IdentityModel.Tokens;
using ReservationsService.API.Models;
using ReservationsService.API.Models.Input;
using ReservationsService.API.Models.Output;

namespace ReservationsService.DB.Extensions;

public static class ReservationDatesExtensions
{
    public static ReservationPeriod ReservationPeriod(this Reservations entry)
    {
        return new ReservationPeriod(entry.StartDate, entry.EndDate);
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

    internal static BlockedDates ToTableEntry(this DateTime date, Guid itemId) => new BlockedDates
    {
        ItemId = itemId,
        Date = date,
    };

    internal static Reservations ToTableEntry(this ReservationRequestInput reservationRequestInput, Guid reservationID, Guid itemId, Guid lenderId, Guid borrowerId)
    {
        return new Reservations
        {
            ReservationId = reservationID,
            ItemId = itemId,
            StartDate = reservationRequestInput.StartDate,
            EndDate = reservationRequestInput.EndDate,
            Status = ReservationStatus.Pending,
            BorrowerId = borrowerId,
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
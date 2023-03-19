using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using ReservationsService.API.Models.Output;

namespace ReservationsService.DB.Extensions;

internal static class ReservationDatesExtensions
{
    internal static ReservationDates ToReservationDates(this Reservations entry)
    {
        return new ReservationDates
        {
            StartDate = entry.From,
            EndDate = entry.To,
        };
    }

    internal static Reservations ToTableEntry(this ReservationDates reservationDates, Guid itemId)
    {
        return new Reservations
        {
            ItemId = itemId,
            From = reservationDates.StartDate,
            To = reservationDates.EndDate,
        };
    }
}
using ReservationsService.API.Models.Output;

namespace ReservationsService.API.Interfaces;

public interface IReservationsServiceBackend
{
    List<ReservationDates> GetReservedDates(Guid itemId, DateTime from, DateTime to);

    void AddReservations(Guid itemId, IEnumerable<ReservationDates> reservationDates);
}
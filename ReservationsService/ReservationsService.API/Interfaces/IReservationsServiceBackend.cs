using ReservationsService.API.Models.Input;
using ReservationsService.API.Models.Output;

namespace ReservationsService.API.Interfaces;

public interface IReservationsServiceBackend
{
    Task<List<ReservationDetails>> GetPendingReservations(Guid itemId, DateTime from, DateTime to);
    Task<List<DateTime>> GetReservedDates(Guid itemId, DateTime startDate, DateTime endDate);
    Task<ReservationRequestResult> AddReservationRequest(Guid itemId, Guid borrowerId, ReservationRequestInput reservationRequestInput);
    Task BlockDates(Guid itemId, IEnumerable<DateTime> dates);
}

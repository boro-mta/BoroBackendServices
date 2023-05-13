using ReservationsService.API.Models.Input;
using ReservationsService.API.Models.Output;

namespace ReservationsService.API.Interfaces;

public interface IReservationsServiceBackend
{
    Task<List<ReservationDetails>> GetPendingReservations(Guid itemId);
    Task BlockDates(Guid itemId, DateTime startDate, DateTime endDate);
    Task<List<DateTime>> GetReservedDates(Guid itemId, DateTime startDate, DateTime endDate);
    Task<ReservationRequestResult> AddReservationRequest(Guid itemId, Guid borrowerId, ReservationRequestInput reservationRequestInput);
}
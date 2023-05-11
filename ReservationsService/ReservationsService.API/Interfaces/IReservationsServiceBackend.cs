using ReservationsService.API.Models.Input;
using ReservationsService.API.Models.Output;

namespace ReservationsService.API.Interfaces;

public interface IReservationsServiceBackend
{
    Task<List<ReservedDates>> GetReservedDates(Guid itemId, DateTime from, DateTime to);

    Task<ReservationRequestResult> AddReservationRequest(Guid itemId, ReservationRequestInput reservationRequestInput);
    Task<List<ReservationDetails>> GetPendingReservations(Guid itemId);
    Task BlockDates(Guid itemId, DateTime startDate, DateTime endDate);
}
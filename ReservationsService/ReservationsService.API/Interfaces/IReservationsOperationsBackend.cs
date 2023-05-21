using ReservationsService.API.Models.Output;

namespace ReservationsService.API.Interfaces;

public interface IReservationsOperationsBackend
{
    Task ApproveAsync(Guid reservationId);
    Task DeclineAsync(Guid reservationId);
    Task CancelAsync(Guid reservationId);
    Task HandOverToBorrowerAsync(Guid reservationId);
    Task ReturnToLenderAsync(Guid reservationId);
    Task<ReservationDetails> GetReservationDetailsAsync(Guid reservationId);
}

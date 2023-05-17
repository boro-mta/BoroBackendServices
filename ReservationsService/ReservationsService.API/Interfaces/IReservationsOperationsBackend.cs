namespace ReservationsService.API.Interfaces;

public interface IReservationsOperationsBackend
{
    Task Approve(Guid reservationId);
    Task Decline(Guid reservationId);
    Task Cancel(Guid reservationId);
    Task HandOverToBorrower(Guid reservationId);
    Task ReturnToLender(Guid reservationId);
}

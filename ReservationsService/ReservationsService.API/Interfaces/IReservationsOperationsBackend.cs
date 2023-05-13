namespace ReservationsService.API.Interfaces;

public interface IReservationsOperationsBackend
{
    Task Approve(string reservationId);
    Task Decline(string reservationId);
    Task Cancel(string reservationId);
    Task HandOverToBorrower(string reservationId);
    Task ReturnToLender(string reservationId);
}

using Boro.EntityFramework.DbContexts.BoroMainDb.Enum;

namespace ReservationsService.API.Exceptions;

public class IllegalReservationOperationException : Exception
{
    public IllegalReservationOperationException(Guid reservationId, ReservationStatus currentStatus, ReservationStatus newStatus) 
        : base($"Illegal Operation - Reservation [{reservationId}] is currently in status: [{currentStatus}] and cannot be set to [{newStatus}]")
    {
        ReservationId = reservationId;
        CurrentStatus = currentStatus;
        NewStatus = newStatus;
    }

    public Guid ReservationId { get; }
    public ReservationStatus CurrentStatus { get; }
    public ReservationStatus NewStatus { get; }
}

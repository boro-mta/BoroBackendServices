using System.Collections.Immutable;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Enum;

public enum ReservationStatus
{
    Canceled = 0,
    Returned = 10,
    Declined = 20,
    Pending = 30,
    Approved = 40,
    Borrowed = 50,
}

public static class Statuses
{
    public static ReservationStatus[] BlockingStatuses { get; } = new ReservationStatus[]
    {
        ReservationStatus.Approved,
        ReservationStatus.Borrowed
    };

    public static ReservationStatus[] ConcludedStatuses { get; } = new ReservationStatus[]
    {
        ReservationStatus.Canceled,
        ReservationStatus.Returned,
        ReservationStatus.Declined
    };

    public static ReservationStatus[] ActiveStatuses { get; } = new ReservationStatus[]
    {
        ReservationStatus.Pending,
        ReservationStatus.Approved,
        ReservationStatus.Borrowed
    };

    public static bool IsBlockingStatus(this ReservationStatus status)
        => BlockingStatuses.Contains(status);

    public static bool IsActiveStatus(this ReservationStatus status)
        => ActiveStatuses.Contains(status);

    public static bool IsConcludedStatus(this ReservationStatus status)
        => ConcludedStatuses.Contains(status);
}

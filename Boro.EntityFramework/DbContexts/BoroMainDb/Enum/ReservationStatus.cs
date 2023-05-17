namespace Boro.EntityFramework.DbContexts.BoroMainDb.Enum;

public enum ReservationStatus
{
    Canceled = 0,
    Returned,
    Declined,
    Pending,
    Approved,
    Borrowed,
}

public static class ReservationStatusExtensions
{
    public static IReadOnlyCollection<ReservationStatus> BlockingStatuses { get; } = new List<ReservationStatus>
    {
        ReservationStatus.Approved,
        ReservationStatus.Borrowed
    };

    public static IReadOnlyCollection<ReservationStatus> ConcludedStatuses { get; } = new List<ReservationStatus>
    {
        ReservationStatus.Canceled,
        ReservationStatus.Returned,
        ReservationStatus.Declined
    };

    public static IReadOnlyCollection<ReservationStatus> ActiveStatuses { get; } = new List<ReservationStatus>
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

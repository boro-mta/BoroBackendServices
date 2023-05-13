using System.Collections.ObjectModel;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Enum;

public enum ReservationStatus
{
    Returned,
    Denied,
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
        ReservationStatus.Returned,
        ReservationStatus.Denied
    };

    public static IReadOnlyCollection<ReservationStatus> ActiveStatuses { get; } = new List<ReservationStatus>
    {
        ReservationStatus.Pending,
        ReservationStatus.Approved,
        ReservationStatus.Borrowed
    };

    public static bool IsBlockingStatus(this ReservationStatus status) 
        => BlockingStatuses.Contains(status);
}

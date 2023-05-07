namespace Boro.EntityFramework.DbContexts.BoroMainDb.Enum;

public enum ReservationStatus
{
    Unknown = 0,
    None,
    Returned,
    Denied,
    Pending,
    Approved,
    Borrowed,
}

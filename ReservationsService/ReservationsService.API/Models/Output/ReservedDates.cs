using Boro.EntityFramework.DbContexts.BoroMainDb.Enum;

namespace ReservationsService.API.Models.Output;

public class ReservedDates
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ReservationStatus Status { get; set; }
    public bool IsBlockedByOwner { get; set; }
}

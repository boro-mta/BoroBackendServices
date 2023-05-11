using Boro.EntityFramework.DbContexts.BoroMainDb.Enum;

namespace ReservationsService.API.Models.Output;

public class ReservationDetails
{
    public Guid ReservationId { get; set; }
    public Guid ItemId { get; set; }
    public Guid BorrowerId { get; set; }
    public Guid LenderId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ReservationStatus Status { get; set; }
}

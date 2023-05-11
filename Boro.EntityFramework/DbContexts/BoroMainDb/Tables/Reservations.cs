using Boro.EntityFramework.DbContexts.BoroMainDb.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

public class Reservations
{
    [Key]
    public Guid ReservationId { get; set; }
    [ForeignKey(nameof(Items))]
    public Guid ItemId { get; set; }
    [ForeignKey(nameof(Users))]
    public Guid BorrowerId { get; set; }
    [ForeignKey(nameof(Users))]
    public Guid LenderId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ReservationStatus Status { get; set; }
}

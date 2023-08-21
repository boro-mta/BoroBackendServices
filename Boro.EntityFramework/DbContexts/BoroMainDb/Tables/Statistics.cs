using System.ComponentModel.DataAnnotations;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables
{
    public class Statistics
    {
        [Key]
        public Guid UserId { get; set; }
        public int AmountOfItems { get; set; }
        public int AmountOfLendings { get; set; }
        public int AmountOfBorrowings { get; set; }
    }
}

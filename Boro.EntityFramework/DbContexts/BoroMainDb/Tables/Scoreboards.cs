using System.ComponentModel.DataAnnotations;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables
{
    public class Scoreboards
    {
        [Key]
        public Guid UserId { get; set; }
        public int AmountOfItems { get; set; }
        public int AmountOfLendings { get; set; }
        public int AmountOfBorrowings { get; set; }
        public int TotalScore { get; set; }
    }
}

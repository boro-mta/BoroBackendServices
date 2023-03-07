using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItemService.DB.DbContexts.Tables
{
    public class ItemServices
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Description")]
        public string? Description { get; set; }
    }
}
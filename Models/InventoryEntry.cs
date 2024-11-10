using System.ComponentModel.DataAnnotations;

namespace glissvinyls_plus.Models
{
    public class InventoryEntry
    {
        [Key]
        public int EntryId { get; set; }
        public DateTime EntryDate { get; set; }
        public int SupplierId { get; set; }
        public float TotalEntry { get; set; }
    }

}

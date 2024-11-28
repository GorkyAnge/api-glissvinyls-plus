using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace glissvinyls_plus.Models
{
    public class InventoryEntry
    {
        [Key]
        public int EntryId { get; set; }
        public DateTime EntryDate { get; set; }
        public int SupplierId { get; set; }
        public float TotalEntry { get; set; }

        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; } = null!;
        public ICollection<EntryDetail> EntryDetails { get; set; } = new List<EntryDetail>();
    }

}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace glissvinyls_plus.Models
{
    public class EntryDetail
    {
        [Key]
        public int EntryDetailId { get; set; }
        public int EntryId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float PurchasePrice { get; set; }

        [ForeignKey("EntryId")]
        public InventoryEntry InventoryEntry { get; set; } = null!;
        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;
    }

}

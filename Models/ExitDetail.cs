using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace glissvinyls_plus.Models
{
    public class ExitDetail
    {
        [Key]
        public int ExitDetailId { get; set; }
        public int ExitId { get; set; } // FK TO InventoryExit
        [ForeignKey("ExitId")]
        public InventoryExit InventoryExit { get; set; } = null!;
        
        public int Quantity { get; set; }
        public float SalePrice { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; } // Clave foránea que referencia al producto
        public Product Product { get; set; }
    }
}

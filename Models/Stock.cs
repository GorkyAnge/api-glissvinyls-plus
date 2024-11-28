using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace glissvinyls_plus.Models
{
    public class Stock
    {
        [Key]
        public int StockId { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int AvailableQuantity { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; } = null!;
    }

}

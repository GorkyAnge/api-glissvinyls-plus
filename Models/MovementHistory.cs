using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace glissvinyls_plus.Models
{
    public class MovementHistory
    {
        [Key]
        public int HistoryId { get; set; }
        public int ProductId { get; set; }
        public DateTime MovementDate { get; set; }
        public string MovementType { get; set; } = null!;
        public int Quantity { get; set; }
        public int WarehouseId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; } = null!;
    }

}

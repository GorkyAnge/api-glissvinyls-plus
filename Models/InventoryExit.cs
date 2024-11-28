using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace glissvinyls_plus.Models
{
    public class InventoryExit
    {
        [Key]
        public int ExitId { get; set; }
        public DateTime ExitDate { get; set; }
        public int ClientId { get; set; }
        public float TotalExit { get; set; }
        public int WarehouseId { get; set; }

        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;

namespace glissvinyls_plus.Models
{
    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = null!;
        public string Address { get; set; } = null!;

        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
        public ICollection<MovementHistory> MovementsHistory { get; set; } = new List<MovementHistory>();
    }

}

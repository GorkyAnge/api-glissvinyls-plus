using System.ComponentModel.DataAnnotations;

namespace glissvinyls_plus.Models
{
    public class Stock
    {
        [Key]
        public int StockId { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int AvailableQuantity { get; set; }
    }

}

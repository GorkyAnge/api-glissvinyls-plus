namespace glissvinyls_plus.Models.DTO
{
    public class MovementHistoryDto
    {
        public int HistoryId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public DateTime MovementDate { get; set; }
        public string MovementType { get; set; } = null!;
        public int Quantity { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = null!;
    }


}

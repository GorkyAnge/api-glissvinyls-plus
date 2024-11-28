namespace glissvinyls_plus.Models.DTO
{
    public class TopRotatedProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalMovements { get; set; } // Cantidad total de movimientos (entradas + salidas)
        public int TotalQuantity { get; set; } // Total de productos movidos
        public int EntryMovements { get; set; } // Cantidad de movimientos de entrada
        public int ExitMovements { get; set; } // Cantidad de movimientos de salida
        public int EntryQuantity { get; set; } // Total de productos ingresados
        public int ExitQuantity { get; set; } // Total de productos retirados
    }

}

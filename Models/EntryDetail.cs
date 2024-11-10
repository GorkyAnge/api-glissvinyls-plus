using System.ComponentModel.DataAnnotations;

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
    }

}

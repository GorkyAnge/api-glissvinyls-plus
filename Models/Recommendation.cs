using System.ComponentModel.DataAnnotations;

namespace glissvinyls_plus.Models
{
    public class Recommendation
    {
        [Key]
        public int RecommendationId { get; set; }
        public int ProductId { get; set; }
        public DateTime RecommendationDate { get; set; }
        public int RecommendedQuantity { get; set; }
        public string Justification { get; set; } = null!;
    }

}

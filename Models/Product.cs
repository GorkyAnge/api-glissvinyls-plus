﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace glissvinyls_plus.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = null!;
        public ICollection<MovementHistory> MovementsHistory { get; set; } = new List<MovementHistory>();
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    }
}

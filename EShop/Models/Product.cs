﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EShop.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        [Required]
        [StringLength(150)]
        public string? ProductName { get; set; }
        [StringLength(30000)]
        public string? ProductDescription { get; set; }
        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public Category? Category { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal ProductPrice { get; set; }

        [Column(TypeName = "decimal(2,2)")]

        public decimal ProductDiscount { get; set; } = 0;
        [StringLength(300)]
        public string? ProductPhoto { get; set; }
        [ForeignKey("Size")]
        public int SizeID { get; set; }
        public Size? Size { get; set; }
        [ForeignKey("Color")]
        public int ColorID { get; set; }
        public Color? Color { get; set; }
        public bool IsTrandy { get; set; }
        public bool IsArrived { get; set; }

    }
}

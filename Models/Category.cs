using System.ComponentModel.DataAnnotations;

namespace EShop.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [StringLength(150)]
        public string? CategoryName { get; set; }
        [StringLength(300)]
        public string? CategoryDescription { get; set; }
        [StringLength(300)]
        public string? CategoryPhoto { get; set; }
        public int CategoryOrder { get; set; }

    }
}

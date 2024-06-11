using System.ComponentModel.DataAnnotations;

namespace EShop.Models
{
    public class Size
    {
        [Key]
        public int SizeID { get; set; }
        [StringLength(10)]
        public string? SizeName { get; set; }
    }
}

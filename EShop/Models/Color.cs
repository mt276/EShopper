using System.ComponentModel.DataAnnotations;

namespace EShop.Models
{
    public class Color
    {
        [Key]
        public int ColorID { get; set; }
        [StringLength(150)]
        public string? ColorName { get; set; }
    }
}

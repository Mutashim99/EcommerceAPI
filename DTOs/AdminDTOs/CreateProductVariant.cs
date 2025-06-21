using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs.AdminDTOs
{
    public class CreateProductVariant
    {
        [Required]
        public string Size { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Stock { get; set; }
        public string? VariantImageUrl { get; set; }

    }
}

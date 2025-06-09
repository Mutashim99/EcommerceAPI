using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs.ProductVariantDTOs
{
    public class ProductVariantResponseDTO
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? VariantImageUrl { get; set; }
    }
}

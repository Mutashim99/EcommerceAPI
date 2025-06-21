using EcommerceAPI.DTOs.CategoryDTOs;
using EcommerceAPI.DTOs.ProductVariantDTOs;
using EcommerceAPI.DTOs.ReviewDTOs;

namespace EcommerceAPI.DTOs.AdminDTOs
{
    public class AdminProductDetailsResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public string? Brand { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        public CategoryResponseDTO Category { get; set; }
        public List<ProductVariantResponseDTO> Variants { get; set; }
        public List<ReviewResponseDTO> Reviews { get; set; }
    }
}

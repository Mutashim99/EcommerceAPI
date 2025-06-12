using EcommerceAPI.DTOs.CategoryDTOs;
using EcommerceAPI.DTOs.ProductVariantDTOs;
using EcommerceAPI.DTOs.ReviewDTOs;

namespace EcommerceAPI.DTOs.ProductDTOs
{
    public class ProductForDisplayResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public string? Brand { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }


        public int HowManySold { get; set; } // Total quantity sold in delivered orders
        public double AvgRating { get; set; } // Average review rating (0-5)
        public int TotalReviews { get; set; } // Total number of reviews

        public CategoryResponseDTO Category { get; set; }
        public List<ProductVariantResponseDTO> Variants { get; set; }
        public List<ReviewResponseDTO> Reviews { get; set; }
    }

}

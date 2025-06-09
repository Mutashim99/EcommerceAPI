using EcommerceAPI.DTOs.ProductDTOs;
using EcommerceAPI.Models;

namespace EcommerceAPI.DTOs.UserFavoriteDTOs
{
    public class FavoriteResponseDTO
    {
        public int Id { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;

        public ProductResponseDTO FavoriteProduct { get; set; } // jo product hua add favorite
    }
}

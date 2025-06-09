using EcommerceAPI.DTOs.ProductDTOs;
using EcommerceAPI.DTOs.UserFavoriteDTOs;

namespace EcommerceAPI.Services.UserManagement.UserFavoriteManagement
{
    public interface IUserFavorite
    {
        Task<ServiceResponse<List<ProductResponseDTO>>> AddToFavoritesAsync(int userId, int productId);
        Task<ServiceResponse<List<ProductResponseDTO>>> RemoveFromFavoritesAsync(int userId, int productId);
        Task<ServiceResponse<List<FavoriteResponseDTO>>> GetFavoritesAsync(int userId);
    }
}

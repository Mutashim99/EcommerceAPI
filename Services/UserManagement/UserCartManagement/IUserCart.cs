using EcommerceAPI.DTOs.UserCartDTOs;

namespace EcommerceAPI.Services.UserManagement.UserCartManagement
{
    public interface IUserCart
    {
        public Task<ServiceResponse<List<CartItemResponseDTO>>> GetCartItemsAsync(int userId);
        public Task<ServiceResponse<CartItemResponseDTO>> AddCartItemAsync(int userId,  CreateCartItemDTO request);
        public Task<ServiceResponse<CartItemResponseDTO>> UpdateCartItemQuantityAsync(int userId, int cartItemId, int newQuantity);
        public Task<ServiceResponse<List<CartItemResponseDTO>>> RemoveCartItemAsync(int userId, int cartItemId);
        Task<ServiceResponse<string>> ClearCartAsync(int userId);
    }

}

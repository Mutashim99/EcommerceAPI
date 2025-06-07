namespace EcommerceAPI.Services.UserManagement.UserCartManagement
{
    public interface IUserCart
    {
        Task<List<CartItemDto>> GetCartItemsAsync(int userId);
        Task AddCartItemAsync(int userId, AddCartItemRequest request);
        Task UpdateCartItemQuantityAsync(int userId, int cartItemId, int newQuantity);
        Task RemoveCartItemAsync(int userId, int cartItemId);
        Task ClearCartAsync(int userId);
    }

}

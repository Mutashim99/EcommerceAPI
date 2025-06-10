using EcommerceAPI.DTOs.OrderDTOs;

namespace EcommerceAPI.Services.UserManagement.UserOrderManagement
{
    public interface IUserOrder
    {
        Task<ServiceResponse<int>> PlaceOrderAsync(int userId, CreateOrderDTO request);
        Task<ServiceResponse<List<CheckoutPreviewResponseDTO>>> GetCheckoutPreviewAsync(int userId, List<int> cartItemIds);
        Task<ServiceResponse<List<UserOrderResponseDTO>>> GetMyOrdersAsync(int userId);
        Task<ServiceResponse<UserOrderDetailsResponseDTO>> GetOrderDetailsAsync(int userId, int orderId);
        Task<ServiceResponse<string>> CancelOrderAsync(int userId, int orderId);
    }
}

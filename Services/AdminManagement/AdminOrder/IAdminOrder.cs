using EcommerceAPI.DTOs.AdminDTOs;

namespace EcommerceAPI.Services.AdminManagement.AdminOrder
{
    public interface IAdminOrder
    {
        // 1. Get all orders (for full list with optional future pagination)
        Task<ServiceResponse<List<AdminOrderResponseDTO>>> GetAllOrdersAsync();

        // 2. Get orders filtered by their status (e.g., "Pending", "Delivered", etc.)
        Task<ServiceResponse<List<AdminOrderResponseDTO>>> GetOrdersByStatusAsync(string status);

        // 3. Get detailed information for a specific order by ID
        Task<ServiceResponse<AdminOrderDetailsResponseDTO>> GetOrderByIdAsync(int orderId);

        // 4. Update the status of a specific order
        Task<ServiceResponse<string>> UpdateOrderStatusAsync(int orderId, string newStatus);
    }
}

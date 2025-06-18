using EcommerceAPI.DTOs.OrderDTOs;
using EcommerceAPI.DTOs.UserProfileDTOs;

namespace EcommerceAPI.DTOs.AdminDTOs
{
    public class AdminOrderDetailsResponseDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }

        public UserProfileDTO User { get; set; }

        public OrderAddressResponseDTO Address { get; set; }
        public List<OrderItemResponseDTO> OrderItems { get; set; }
        public string? OnlinePaymentProofImageURL { get; set; }
    }

}

using EcommerceAPI.DTOs.UserProfileDTOs;

namespace EcommerceAPI.DTOs.AdminDTOs
{
    public class AdminOrderResponseDTO
    {
        public int Id { get; set; } // Order ID
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }

        public UserProfileDTO User { get; set; }
    }


}

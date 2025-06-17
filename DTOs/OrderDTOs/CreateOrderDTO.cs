using EcommerceAPI.DTOs.UserAddressDTOs;

namespace EcommerceAPI.DTOs.OrderDTOs
{
    public class CreateOrderDTO
    {
        public List<int> SelectedCartItemIds { get; set; }
        public int AddressId { get; set; }
        public CreateAddressDto? NewAddress { get; set; }
        public string PaymentMethod { get; set; } // e.g. "Cash On Delivery", "Online Transfer"
        public string? OnlinePaymentProofImageURL { get; set; }
    }

}

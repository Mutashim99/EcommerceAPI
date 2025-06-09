using EcommerceAPI.DTOs.ProductDTOs;

namespace EcommerceAPI.DTOs.UserCartDTOs
{
    public class CartItemResponseDTO
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime Added { get; set; }

        public ProductResponseDTO? Product { get; set; }
    }

}

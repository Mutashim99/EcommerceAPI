namespace EcommerceAPI.DTOs.OrderDTOs
{
    public class OrderItemResponseDTO
    {
        public string ProductName { get; set; }
        public string ImageURL { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchaseTime { get; set; }
    }

}

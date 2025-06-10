namespace EcommerceAPI.DTOs.OrderDTOs
{
    public class CheckoutPreviewResponseDTO
    {
        public int CartItemId { get; set; }
        public string ProductName { get; set; }
        public string ImageURL { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public int VariantId { get; set; }
        public string VariantColor { get; set; }
        public string VariantSize { get; set; }
    }

}

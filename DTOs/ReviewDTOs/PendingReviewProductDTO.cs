namespace EcommerceAPI.DTOs.ReviewDTOs
{
    public class PendingReviewProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageURL { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchaseTime { get; set; }
        public decimal OrderTotalAmount { get; set; }
        public string OrderStatus { get; set; }
    }

}

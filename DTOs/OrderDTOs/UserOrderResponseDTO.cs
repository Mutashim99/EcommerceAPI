namespace EcommerceAPI.DTOs.OrderDTOs
{
    public class UserOrderResponseDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
    }

}

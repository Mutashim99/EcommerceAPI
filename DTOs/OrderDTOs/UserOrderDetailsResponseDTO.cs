namespace EcommerceAPI.DTOs.OrderDTOs
{
public class UserOrderDetailsResponseDTO
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentMethod { get; set; }
    public OrderAddressResponseDTO ShippingAddress { get; set; }
    public List<OrderItemResponseDTO> Items { get; set; }
}

}

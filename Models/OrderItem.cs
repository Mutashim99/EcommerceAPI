using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal PriceAtPurchaseTime { get; set; } // this will show the price that was at the time of shopping, if admin changes the price of an item it wont effect in the previous orders


        public int VariantId { get; set; } // Store selected variant that was in the order
        public ProductVariant Variant { get; set; }


        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}

using EcommerceAPI.Constants;
using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string OrderStatus { get; set; } = OrderStatuses.Pending;

        public decimal TotalAmount { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;
        public string? OnlinePaymentProofImageURL { get; set; }

        public int UserId { get; set; }     // FK to User (who placed the order)
        public User User { get; set; }


        public int AddressId { get; set; }// FK to Address (shipping location)
        public Address Address { get; set; } //Order and Address	1️ ⟶ 1️		Each order is linked to a single address

        public List<OrderItem> OrderItems { get; set; }       // Navigation to OrderItems
    }
}

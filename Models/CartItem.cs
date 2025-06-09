using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        [Range(1, int.MaxValue)]
        [Required]
        public int Quantity { get; set; }
        public DateTime Added { get; set; } = DateTime.Now;


        public int UserId { get; set; } // 1 user can have multiple cart items so UserId here.(FK)
        public User User { get; set; } // Navifation property for User


        public int ProductId { get; set; } // 1 product can be in many CartItem so 1 to many (FK)
        public Product Product { get; set; }   // the product that is in the cart


        public int ProductVariantId { get; set; }
        public ProductVariant Variant { get; set; }


    }
}

using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required] 
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; } //will be implemented through BCrypt
        public string Role { get; set; } = "Customer"; //Default is Customer when a user is created

        public List<Order>? Orders { get; set; } // 1 user can have multiple orders
        public List<CartItem>? CartItems { get; set; } // 1 user can have multiple cartitems
        public List<Address>? Addresses { get; set; } // 1 user can have multiple Address (Permanent/Home/Office)
        public List<Review>? Reviews { get; set; }  // 1 user can have multiple Reviews
    }
}

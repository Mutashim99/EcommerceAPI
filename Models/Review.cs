using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        [Range(1, 5)]
        [Required]
        public int Rating { get; set; }
        public string? Comment { get; set; } //comment is optional 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public int UserId { get; set; } // link to  User(foreign Key) that this user has written this review
        public User user { get; set; } // Navigation property for User


        public int ProductId { get; set; } // Link to Product that this product has this review
        public Product Product { get; set; } // Navigation to that product
    }
}

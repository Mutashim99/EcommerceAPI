using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
       
        [Required]
        public string ImageURL { get; set; } // 1 Image url for now, can add another model if we want to add multiple images and can link these two models like 1 product can have multiple images so Product key on images side
        
        public string? Brand { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Date and time when the prodcut was created
        public bool IsActive { get; set; } = true; // Instead of Deleting a product mark it Inactive to display only active products


        public List<Review>? Reviews { get; set; }  // 1 Product can have multiple Reviews

        public int CategoryId { get; set; } // 1 category can have multiple products so Category ID and nav property here
        public Category Category { get; set; }

        public List<OrderItem>? OrderItems { get; set; } // to connect OrderItems table and Products table like many users can order the same product so 1 product can have many OrderItems
        // isko aese smjho ke 1 order mein multiple orderitems ho skte to har item koi na koi product he aur product multiple orderitems mein aa skta
        //ye admin panel ke lye useful he because of admin panel mein koi bhi product aur us product ki order history le skte ek saath useful for analytics
        public List<CartItem>? CartItems { get; set; } // to connect OrderItems table and Products table like many users can add the same product to card so 1 product can have many CartItems
        //useful for checking how many peoples have added a particular item to their cart for analytics

        public List<FavoriteItem>? FavoriteItems { get; set; }

        public List<ProductVariant>? Variants { get; set; }
    }
}

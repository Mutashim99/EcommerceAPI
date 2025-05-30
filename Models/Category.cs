using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public List<Product>? Products { get; set; } // 1 category can have many products so navigation property for all the products in a particular category
    }
}

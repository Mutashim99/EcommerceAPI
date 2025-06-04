using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models
{
    public class ProductVariant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Size { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Stock { get; set; }
        public string? VariantImageUrl { get; set; }


        public int ProductId { get; set; }
        public Product Product { get; set; }
    }

}

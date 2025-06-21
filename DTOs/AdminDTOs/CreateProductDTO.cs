using EcommerceAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs.AdminDTOs
{
    public class CreateProductDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImageURL { get; set; }
        public string? Brand { get; set; }
        public bool IsActive { get; set; } = true;
        public int CategoryId { get; set; }
        [Required]
        public List<CreateProductVariant> Variants { get; set; }
    }
}

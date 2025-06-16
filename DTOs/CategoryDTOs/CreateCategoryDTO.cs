using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs.CategoryDTOs
{
    public class CreateCategoryDTO
    {
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}

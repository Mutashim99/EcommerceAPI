using EcommerceAPI.DTOs.CategoryDTOs;

namespace EcommerceAPI.DTOs.AdminDTOs
{
    public class AdminProductListResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public string? Brand { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public CategoryResponseDTO Category { get; set; }
    }
}

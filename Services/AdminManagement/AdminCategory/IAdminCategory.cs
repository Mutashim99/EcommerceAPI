using EcommerceAPI.DTOs.CategoryDTOs;

namespace EcommerceAPI.Services.AdminManagement.AdminCategory
{
    public interface IAdminCategory
    {
        Task<ServiceResponse<List<CategoryResponseDTO>>> GetAllCategoriesAsync();
        Task<ServiceResponse<CategoryResponseDTO>> GetCategoryByIdAsync(int id);
        Task<ServiceResponse<CategoryResponseDTO>> CreateCategoryAsync(CreateCategoryDTO dto);
        Task<ServiceResponse<CategoryResponseDTO>> UpdateCategoryAsync(int id, CreateCategoryDTO dto);
        Task<ServiceResponse<string>> DeleteCategoryAsync(int id);
    }
}

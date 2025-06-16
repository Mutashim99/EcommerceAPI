using AutoMapper;
using EcommerceAPI.DTOs.CategoryDTOs;

namespace EcommerceAPI.Services.AdminManagement.AdminCategory
{
    public class AdminCategoryService : IAdminCategory
    {
        private readonly AppDbContext db;
        private readonly IMapper mapper;

        public AdminCategoryService(AppDbContext db, IMapper mapper) 
        {
            this.db = db;
            this.mapper = mapper;
        }
        public Task<ServiceResponse<CategoryResponseDTO>> CreateCategoryAsync(CreateCategoryDTO dto)
        {

            throw new NotImplementedException();
        }

        public Task<ServiceResponse<string>> DeleteCategoryAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<CategoryResponseDTO>>> GetAllCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<CategoryResponseDTO>> GetCategoryByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<CategoryResponseDTO>> UpdateCategoryAsync(int id, CreateCategoryDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}

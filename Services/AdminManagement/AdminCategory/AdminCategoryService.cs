using AutoMapper;
using EcommerceAPI.DTOs.CategoryDTOs;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ServiceResponse<CategoryResponseDTO>> CreateCategoryAsync(CreateCategoryDTO dto)
        {
            if (dto == null)
            {
                return new ServiceResponse<CategoryResponseDTO>()
                {
                    Message = "Data is Null",
                    Success = false,
                    Data = null
                };
            }
            var MappedCategory = mapper.Map<Category>(dto);
            await db.AddAsync(MappedCategory);
            await db.SaveChangesAsync();
            var mappedIntoResponse = mapper.Map<CategoryResponseDTO>(MappedCategory);
            return new ServiceResponse<CategoryResponseDTO>()
            {
                Message = "Category added Succesfully",
                Success = true,
                Data = mappedIntoResponse
            };
        }

        public async Task<ServiceResponse<string>> DeleteCategoryAsync(int categoryId)
        {
            var categoryFromDb = await db.Categories.FirstOrDefaultAsync(x=> x.Id == categoryId);
            if (categoryFromDb == null)
            {
                return new ServiceResponse<string>()
                {
                    Message = "cant find the category with given Id",
                    Success = false,
                    Data = null
                };
            }
            db.Remove(categoryFromDb);
           await db.SaveChangesAsync();
            return new ServiceResponse<string>()
            {
                Message = "Category deleted succesfully",
                Success = true,
                Data = "Category deleted succesfully"
            };
        }

        public async Task<ServiceResponse<List<CategoryResponseDTO>>> GetAllCategoriesAsync()
        {
            var allCategories = await db.Categories.ToListAsync();

            var mappedCategories = mapper.Map<List<CategoryResponseDTO>>(allCategories);

            return new ServiceResponse<List<CategoryResponseDTO>>
            {
                Message = "All Categories Fetched",
                Success = true,
                Data = mappedCategories
            };
        }

        public async Task<ServiceResponse<CategoryResponseDTO>> GetCategoryByIdAsync(int categoryId)
        {
            var categoryById = await db.Categories.FirstOrDefaultAsync(x=> x.Id == categoryId);
            if (categoryById == null)
            {
                return new ServiceResponse<CategoryResponseDTO>()
                {
                    Message = "cant find the category with given Id",
                    Success = false,
                    Data = null
                };
            }
            var mappedForResponse = mapper.Map<CategoryResponseDTO>(categoryById);
            return new ServiceResponse<CategoryResponseDTO>
            {
                Message = "Category Fetched!",
                Success = true,
                Data = mappedForResponse
            };
        }

        public async Task<ServiceResponse<CategoryResponseDTO>> UpdateCategoryAsync(int categoryId, CreateCategoryDTO dto)
        {
            var categoryById = await db.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
            if (categoryById == null)
            {
                return new ServiceResponse<CategoryResponseDTO>()
                {
                    Message = "cant find the category with given Id",
                    Success = false,
                    Data = null
                };
            }
            categoryById.Name = dto.Name;
            categoryById.Description = dto.Description;
            await db.SaveChangesAsync();
            var mappedIntoResponse = mapper.Map<CategoryResponseDTO>(categoryById);
            return new ServiceResponse<CategoryResponseDTO>
            {
                Message = "Category Updated successfully!",
                Success = true,
                Data = mappedIntoResponse

            };
        }
    }
}

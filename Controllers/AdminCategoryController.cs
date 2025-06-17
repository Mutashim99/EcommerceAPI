using EcommerceAPI.DTOs.CategoryDTOs;
using EcommerceAPI.Services.AdminManagement.AdminCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminCategoryController : ControllerBase
    {
        private readonly IAdminCategory _adminCategoryService;

        public AdminCategoryController(IAdminCategory adminCategoryService)
        {
            _adminCategoryService = adminCategoryService;
        }

        [HttpPost("CreateCategory")]
        public async Task<ActionResult<CategoryResponseDTO>> CreateCategory(CreateCategoryDTO dto)
        {
            var result = await _adminCategoryService.CreateCategoryAsync(dto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<ActionResult<string>> DeleteCategory(int id)
        {
            var result = await _adminCategoryService.DeleteCategoryAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<List<CategoryResponseDTO>>> GetAllCategories()
        {
            var result = await _adminCategoryService.GetAllCategoriesAsync();
            return Ok(result.Data);
        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<ActionResult<CategoryResponseDTO>> GetCategoryById(int id)
        {
            var result = await _adminCategoryService.GetCategoryByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        [HttpPut("UpdateCategory/{id}")]
        public async Task<ActionResult<CategoryResponseDTO>> UpdateCategory(int id, CreateCategoryDTO dto)
        {
            var result = await _adminCategoryService.UpdateCategoryAsync(id, dto);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }
    }
}

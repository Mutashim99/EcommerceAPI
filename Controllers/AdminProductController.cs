using EcommerceAPI.DTOs.AdminDTOs;
using EcommerceAPI.Services.AdminManagement.AdminProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminProductController : ControllerBase
    {
        private readonly IAdminProduct adminProduct;

        public AdminProductController(IAdminProduct adminProduct)
        {
            this.adminProduct = adminProduct;
        }

        [HttpPost("create")]
        public async Task<ActionResult<string>> CreateProduct([FromBody] CreateProductDTO dto)
        {
            var result = await adminProduct.CreateProductAsync(dto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<AdminProductListResponseDTO>>> GetAllProducts()
        {
            var result = await adminProduct.GetAllProductsAsync();
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdminProductDetailsResponseDTO>> GetProductById(int id)
        {
            var result = await adminProduct.GetProductByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<string>> UpdateProduct(int id, [FromBody] CreateProductDTO dto)
        {
            var result = await adminProduct.UpdateProductAsync(id, dto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpPut("DeactiveProduct/{ProductId}")]
        public async Task<IActionResult> DeactiveProductAsync(int ProductId)
        {
            var result = await adminProduct.DeActiveProductAsync(ProductId);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }
        [HttpPut("ActiveProduct/{ProductId}")]
        public async Task<IActionResult> ActiveProductAsync(int ProductId)
        {
            var result = await adminProduct.ActiveProductAsync(ProductId);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }
    }
}

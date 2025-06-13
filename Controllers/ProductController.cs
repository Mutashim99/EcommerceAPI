using EcommerceAPI.DTOs.CategoryDTOs;
using EcommerceAPI.DTOs.ProductDTOs;
using EcommerceAPI.Services.Product;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct productService;

        public ProductController(IProduct productService)
        {
            this.productService = productService;
        }

        [HttpGet("AllProducts")]
        public async Task<ActionResult<List<ProductForDisplayResponseDTO>>> GetAllProducts()
        {
            var result = await productService.GetAllProductsAsync();
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("NewArrivals")]
        public async Task<ActionResult<List<ProductForDisplayResponseDTO>>> GetNewArrivals()
        {
            var result = await productService.GetNewArrivalsAsync();
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ProductForDisplayResponseDTO>> GetProductById(int productId)
        {
            var result = await productService.GetProductByIdAsync(productId);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("Category/{categoryName}")]
        public async Task<ActionResult<List<ProductForDisplayResponseDTO>>> GetProductsByCategory(string categoryName)
        {
            var result = await productService.GetProductsByCategoryAsync(categoryName);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("Brand/{brandName}")]
        public async Task<ActionResult<List<ProductForDisplayResponseDTO>>> GetProductsByBrand(string brandName)
        {
            var result = await productService.GetProductsByBrandAsync(brandName);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("{productId}/Related")]
        public async Task<ActionResult<List<ProductForDisplayResponseDTO>>> GetRelatedProducts(int productId)
        {
            var result = await productService.GetRelatedProductsAsync(productId);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("GetCategories")]
        public async Task<ActionResult<List<CategoryResponseDTO>>> GetAllCategories()
        {
            var result = await productService.GetAllCategoriesAsync();
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }
    }
}

using EcommerceAPI.DTOs.CategoryDTOs;
using EcommerceAPI.DTOs.ProductDTOs;

namespace EcommerceAPI.Services.Product
{
    public interface IProduct
    {
        Task<ServiceResponse<List<ProductForDisplayResponseDTO>>> GetAllProductsAsync();
        Task<ServiceResponse<ProductForDisplayResponseDTO>> GetProductByIdAsync(int productId);
        Task<ServiceResponse<List<CategoryResponseDTO>>> GetAllCategoriesAsync();
        Task<ServiceResponse<List<ProductForDisplayResponseDTO>>> GetProductsByCategoryAsync(string categoryName);
        Task<ServiceResponse<List<ProductForDisplayResponseDTO>>> GetProductsByBrandAsync(string brandName);
        Task<ServiceResponse<List<ProductForDisplayResponseDTO>>> GetNewArrivalsAsync();
        Task<ServiceResponse<List<ProductForDisplayResponseDTO>>> GetRelatedProductsAsync(int productId);
    }
}

using EcommerceAPI.DTOs.CategoryDTOs;
using EcommerceAPI.DTOs.ProductDTOs;

namespace EcommerceAPI.Services.Product
{
    public interface IProduct
    {
        Task<ServiceResponse<List<ProductResponseDTO>>> GetAllProductsAsync();
        Task<ServiceResponse<ProductResponseDTO>> GetProductByIdAsync(int productId);
        Task<ServiceResponse<List<CategoryResponseDTO>>> GetAllCategoriesAsync();
        Task<ServiceResponse<List<ProductResponseDTO>>> GetProductsByCategoryAsync(string categoryName);
        Task<ServiceResponse<List<ProductResponseDTO>>> GetProductsByBrandAsync(string brandName);
        Task<ServiceResponse<List<ProductResponseDTO>>> GetPopularProductsAsync();
        Task<ServiceResponse<List<ProductResponseDTO>>> GetNewArrivalsAsync();
        Task<ServiceResponse<List<ProductResponseDTO>>> GetRelatedProductsAsync(int productId);
    }
}

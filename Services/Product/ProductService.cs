using EcommerceAPI.DTOs.CategoryDTOs;
using EcommerceAPI.DTOs.ProductDTOs;

namespace EcommerceAPI.Services.Product
{
    public class ProductService : IProduct
    {
        public Task<ServiceResponse<List<CategoryResponseDTO>>> GetAllCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<ProductResponseDTO>>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<ProductResponseDTO>>> GetNewArrivalsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<ProductResponseDTO>>> GetPopularProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<ProductResponseDTO>> GetProductByIdAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<ProductResponseDTO>>> GetProductsByBrandAsync(string brandName)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<ProductResponseDTO>>> GetProductsByCategoryAsync(string categoryName)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<ProductResponseDTO>>> GetRelatedProductsAsync(int productId)
        {
            throw new NotImplementedException();
        }
    }
}

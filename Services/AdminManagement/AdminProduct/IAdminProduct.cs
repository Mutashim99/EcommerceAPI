using EcommerceAPI.DTOs.AdminDTOs;

namespace EcommerceAPI.Services.AdminManagement.AdminProduct
{
    public interface IAdminProduct
    {
        public Task<ServiceResponse<string>> CreateProductAsync(CreateProductDTO createProduct);
        Task<ServiceResponse<List<AdminProductListResponseDTO>>> GetAllProductsAsync();
        Task<ServiceResponse<AdminProductDetailsResponseDTO>> GetProductByIdAsync(int productId);
        Task<ServiceResponse<string>> UpdateProductAsync(int productId,CreateProductDTO updateProductDTO);


    }
}

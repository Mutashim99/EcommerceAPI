using AutoMapper;
using EcommerceAPI.DTOs.AdminDTOs;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services.AdminManagement.AdminProduct
{
    public class AdminProductService : IAdminProduct
    {
        private readonly AppDbContext db;
        private readonly IMapper mapper;

        public AdminProductService(AppDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<ServiceResponse<string>> CreateProductAsync(CreateProductDTO createProduct)
        {
            if (createProduct == null)
            {
                return new ServiceResponse<string>
                {
                    Message = "Create Product object is null",
                    Data = null,
                    Success = false
                };
            }

 
            var mappedIntoProduct = mapper.Map<EcommerceAPI.Models.Product>(createProduct);
            await db.Products.AddAsync(mappedIntoProduct);
            await db.SaveChangesAsync();
            return new ServiceResponse<string>
            {
                Message = "Product Created",
                Data = $"Product : {mappedIntoProduct.Name}Succesfully Created ",
                Success = true
            };
        }

        public async Task<ServiceResponse<List<AdminProductListResponseDTO>>> GetAllProductsAsync()
        {
            var allProducts = await db.Products
                .Include(x => x.Category)
                .ToListAsync();
            var mappedIntoDTO = mapper.Map<List<AdminProductListResponseDTO>>(allProducts);
            return new ServiceResponse<List<AdminProductListResponseDTO>>
            {
                Data = mappedIntoDTO,
                Success = true,
                Message = "All products fetched succesfully"
            };
            
        }

        public async Task<ServiceResponse<AdminProductDetailsResponseDTO>> GetProductByIdAsync(int productId)
        {
            var productById = await db.Products
                .Include(x=> x.Variants)
                .Include(x=> x.Reviews)
                .Include(x=> x.Category)
                .FirstOrDefaultAsync(x=> x.Id == productId);
            if(productById == null)
            {
                return new ServiceResponse<AdminProductDetailsResponseDTO> { Success = false , Message =$"Cant find any product with the given ID : {productId}"};
            }
            var mappedProduct = mapper.Map<AdminProductDetailsResponseDTO>(productById);
            return new ServiceResponse<AdminProductDetailsResponseDTO>
            {
                Message = $"Product Fetched for the given ID : {productId}",
                Data = mappedProduct,
                Success = true,
            };
        }

        public async Task<ServiceResponse<string>> UpdateProductAsync(int productId, CreateProductDTO updateProductDTO)
        {
            if (productId <= 0 || updateProductDTO == null)
            {
                return new ServiceResponse<string>
                {
                    Data = null,
                    Message = "Product Id or update product is null",
                    Success = false
                };
            }

            var productFromDb = await db.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if (productFromDb == null)
            {
                return new ServiceResponse<string>
                {
                    Data = null,
                    Message = "cant find any product with the given id",
                    Success = false
                };
            }
            mapper.Map(updateProductDTO, productFromDb);
            await db.SaveChangesAsync();

            return new ServiceResponse<string>
            {
                Data = "Product updated succesfully",
                Message = "Product updated succesfully",
                Success = true
            };

        }

        public async Task<ServiceResponse<string>> DeActiveProductAsync(int productId)
        {
            var productFromDB = await db.Products.FindAsync(productId);
            if (productFromDB == null)
            {
                return new ServiceResponse<string>
                {
                    Data = null,
                    Message = "Cant find any product with given id",
                    Success = false
                };
            }
            productFromDB.IsActive = false;
            await db.SaveChangesAsync();
            return new ServiceResponse<string>
            {
                Message = "Product Deactivated successfully",
                Data = "Product Deactivated successfully",
                Success = true
            };
        }
        public async Task<ServiceResponse<string>> ActiveProductAsync(int productId)
        {
            var productFromDB = await db.Products.FindAsync(productId);
            if (productFromDB == null)
            {
                return new ServiceResponse<string>
                {
                    Data = null,
                    Message = "Cant find any product with given id",
                    Success = false
                };
            }
            productFromDB.IsActive = true;
            await db.SaveChangesAsync();
            return new ServiceResponse<string>
            {
                Message = "Product activated successfully",
                Data = "Product activated successfully",
                Success = true
            };
        }

    }
}

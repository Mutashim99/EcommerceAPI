using AutoMapper;
using EcommerceAPI.DTOs.CategoryDTOs;
using EcommerceAPI.DTOs.ProductDTOs;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services.Product
{
    public class ProductService : IProduct
    {
        private readonly AppDbContext db;
        private readonly IMapper mapper;
        public ProductService(AppDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        private async Task<List<ProductForDisplayResponseDTO>> SetExtraFields(List<ProductForDisplayResponseDTO> productDTO)
        {
            foreach (var productDto in productDTO)
            {

                var soldCount = await db.OrderItems
                    .Where(oi => oi.ProductId == productDto.Id && oi.Order.OrderStatus == "Delivered")
                    .SumAsync(oi => (int?)oi.Quantity) ?? 0;

                productDto.HowManySold = soldCount;


                if (productDto.Reviews != null && productDto.Reviews.Count > 0)
                {
                    productDto.TotalReviews = productDto.Reviews.Count;
                    productDto.AvgRating = Math.Round(productDto.Reviews.Average(r => r.Rating), 1);
                }
                else
                {
                    productDto.TotalReviews = 0;
                    productDto.AvgRating = 0;
                }
            }

            return productDTO;
        }

        public async Task<ServiceResponse<List<CategoryResponseDTO>>> GetAllCategoriesAsync()
        {
            var allCategories = await db.Categories.ToListAsync();
            var categoriesMapped = mapper.Map<List<CategoryResponseDTO>>(allCategories);
            return new ServiceResponse<List<CategoryResponseDTO>>
            {
                Data = categoriesMapped,
                Message = "All Categories Fetched",
                Success = true
            };
        }

        public async Task<ServiceResponse<List<ProductForDisplayResponseDTO>>> GetAllProductsAsync()
        {
            var allProducts = await db.Products
                .Include(p => p.Category)
                .Include(p => p.Reviews)
                .Include(p => p.Variants)
                .ToListAsync();

            var mappedProducts = mapper.Map<List<ProductForDisplayResponseDTO>>(allProducts);

            
            var mappedWithExtraFields = await SetExtraFields(mappedProducts);

            return new ServiceResponse<List<ProductForDisplayResponseDTO>>
            {
                Data = mappedWithExtraFields,
                Message = "All products fetched successfully",
                Success = true
            };
        }


        public async Task<ServiceResponse<List<ProductForDisplayResponseDTO>>> GetNewArrivalsAsync()
        {
            var AllNewArrivals = await db.Products
                .Include(x=> x.Variants)
                .Include(x=> x.Category)
                .Include(x=> x.Reviews)
                .OrderByDescending(x=> x.CreatedAt)
                .ToListAsync();

            var mappedProducts = mapper.Map<List<ProductForDisplayResponseDTO>>(AllNewArrivals);

            var mappedWithExtraFields = await SetExtraFields(mappedProducts);

            return new ServiceResponse<List<ProductForDisplayResponseDTO>>
            {
                Data = mappedWithExtraFields,
                Message = "All New Arrivals fetched successfully",
                Success = true
            };

        }



        public async Task<ServiceResponse<ProductForDisplayResponseDTO>> GetProductByIdAsync(int productId)
        {
            var productById = await db.Products
                .Include(x=> x.Variants)
                .Include(x=> x.Category)
                .Include(x=> x.Reviews)
                .FirstOrDefaultAsync(x=> x.Id == productId);

            if(productById == null)
            {
                return new ServiceResponse<ProductForDisplayResponseDTO>
                {
                    Success = false,
                    Message = "cant find any product with the given Id",
                    Data = null
                };
            }
            var mappedIntoDTO = mapper.Map<ProductForDisplayResponseDTO>(productById);
            //mapping extra fields

            int soldCount = await db.OrderItems
                .Where(oi => oi.ProductId == productId && oi.Order.OrderStatus == "Delivered")
                .SumAsync(oi => (int?)oi.Quantity) ?? 0;


            if (mappedIntoDTO.Reviews != null && mappedIntoDTO.Reviews.Count >0)
            {
                mappedIntoDTO.TotalReviews = mappedIntoDTO.Reviews.Count;
                mappedIntoDTO.AvgRating = Math.Round(mappedIntoDTO.Reviews.Average(x => x.Rating));
            }
            else
            {
                mappedIntoDTO.TotalReviews = 0;
                mappedIntoDTO.AvgRating = 0;

            }

            return new ServiceResponse<ProductForDisplayResponseDTO>
            {
                Data = mappedIntoDTO,
                Message = "Product Fetched Succesfully",
                Success = true
            };
        }

        public async Task<ServiceResponse<List<ProductForDisplayResponseDTO>>> GetProductsByBrandAsync(string brandName)
        {
            var productsByBrand = await db.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Include(p => p.Reviews)
                .Where(p => p.Brand != null && p.Brand == brandName)
                .ToListAsync();

            if (productsByBrand == null || !productsByBrand.Any())
            {
                return new ServiceResponse<List<ProductForDisplayResponseDTO>>
                {
                    Success = false,
                    Message = $"No products found for brand '{brandName}'",
                    Data = null
                };
            }

            var mappedProducts = mapper.Map<List<ProductForDisplayResponseDTO>>(productsByBrand);
            var mappedWithExtraFields = await SetExtraFields(mappedProducts);

            return new ServiceResponse<List<ProductForDisplayResponseDTO>>
            {
                Success = true,
                Message = $"Products from brand '{brandName}' fetched successfully",
                Data = mappedWithExtraFields
            };
        }


        public async Task<ServiceResponse<List<ProductForDisplayResponseDTO>>> GetProductsByCategoryAsync(string categoryName)
        {
            var productsByCategory = await db.Products
                .Include(x => x.Variants)
                .Include(x => x.Category)
                .Include(x => x.Reviews)
                .Where(x=> x.Category.Name == categoryName).ToListAsync();

           

            if (productsByCategory.Count == 0)
            {
                return new ServiceResponse<List<ProductForDisplayResponseDTO>>
                {
                    Success = false,
                    Message = "No related products found for the given productId",
                    Data = null
                };
            }

            var mappedProductsintoDTO = mapper.Map<List<ProductForDisplayResponseDTO>>(productsByCategory);

            var mappedWithExtraFields = await SetExtraFields(mappedProductsintoDTO);

            return new ServiceResponse<List<ProductForDisplayResponseDTO>>
            {
                Data = mappedWithExtraFields,
                Message = "All products fetched for the given category successfully",
                Success = true
            };
        }


        public async Task<ServiceResponse<List<ProductForDisplayResponseDTO>>> GetRelatedProductsAsync(int productId)
        {
            var categoryIdOfCurrentProduct = await db.Products
                .Where(x => x.Id == productId)
                .Select(x => x.CategoryId)
                .FirstOrDefaultAsync();

            var relatedProducts = await db.Products
                .Include(x => x.Variants)
                .Include(x => x.Category)
                .Include(x => x.Reviews)
                .Where(x => x.CategoryId == categoryIdOfCurrentProduct && x.Id != productId)
                .ToListAsync();

            if (relatedProducts.Count == 0)
            {
                return new ServiceResponse<List<ProductForDisplayResponseDTO>>
                {
                    Success = false,
                    Message = "No related products found for the given productId",
                    Data = null
                };
            }

            var mappedIntoDto = mapper.Map<List<ProductForDisplayResponseDTO>>(relatedProducts);

            var mappedWithExtraFields = await SetExtraFields(mappedIntoDto);

            return new ServiceResponse<List<ProductForDisplayResponseDTO>>
            {
                Data = mappedWithExtraFields,
                Message = "All related products fetched successfully",
                Success = true
            };
        }

    }
}

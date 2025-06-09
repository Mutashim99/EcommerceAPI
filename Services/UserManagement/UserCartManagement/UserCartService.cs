using AutoMapper;
using EcommerceAPI.DTOs.UserAddressDTOs;
using EcommerceAPI.DTOs.UserCartDTOs;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services.UserManagement.UserCartManagement
{
    public class UserCartService : IUserCart
    {
        private readonly AppDbContext db;
        private readonly IMapper mapper;
        public UserCartService(AppDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
        public async Task<ServiceResponse<List<CartItemResponseDTO>>> GetCartItemsAsync(int userId)
        {
            var userCartItems = await db.CartItems
                    .Where(x => x.UserId == userId)
                    .Include(x => x.Product)
                        .ThenInclude(p => p.Category)
                    .Include(x => x.Product)
                        .ThenInclude(p => p.Variants)
                    .Include(x => x.Product)
                        .ThenInclude(p => p.Reviews)
                     .OrderBy(x => x.Added)
                    .ToListAsync();

            var ResponseCartItems = mapper.Map<List<CartItemResponseDTO>>(userCartItems);

            return new ServiceResponse<List<CartItemResponseDTO>>
            {
                Data = ResponseCartItems,
                Message = "User Cart Items fetched",
                Success = true
            };
            
        }


        public async Task<ServiceResponse<CartItemResponseDTO>> AddCartItemAsync(int userId, CreateCartItemDTO request)
        {
            if (request == null)
            {
                return new ServiceResponse<CartItemResponseDTO>
                {
                    Data = null,
                    Message = "Invalid request data.",
                    Success = false
                };
            }

            var product = await db.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == request.ProductId && p.IsActive);

            if (product == null)
            {
                return new ServiceResponse<CartItemResponseDTO>
                {
                    Data = null,
                    Message = "Product not found or inactive.",
                    Success = false
                };
            }

            var variant = product.Variants.FirstOrDefault(v => v.Id == request.ProductVariantId);
            if (variant == null)
            {
                return new ServiceResponse<CartItemResponseDTO>
                {
                    Data = null,
                    Message = "Invalid product variant.",
                    Success = false
                };
            }

            var existingCartItem = await db.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId &&
                                          c.ProductId == request.ProductId &&
                                          c.ProductVariantId == request.ProductVariantId);

            if (existingCartItem != null)
            {
                int newTotalQuantity = existingCartItem.Quantity + request.Quantity;

                
                if (variant.Stock < newTotalQuantity)
                {
                    return new ServiceResponse<CartItemResponseDTO>
                    {
                        Data = null,
                        Message = "Not enough stock available for this variant.",
                        Success = false
                    };
                }

                return await UpdateCartItemQuantityAsync(userId, existingCartItem.Id, newTotalQuantity);
            }

            if (variant.Stock < request.Quantity)
            {
                return new ServiceResponse<CartItemResponseDTO>
                {
                    Data = null,
                    Message = "Not enough stock for this variant.",
                    Success = false
                };
            }

            var cartItem = mapper.Map<CartItem>(request);
            cartItem.UserId = userId;

            await db.CartItems.AddAsync(cartItem);
            await db.SaveChangesAsync();

            var cartItemWithProduct = await db.CartItems
                .Where(c => c.Id == cartItem.Id)
                .Include(c => c.Product)
                    .ThenInclude(p => p.Category)
                .Include(c => c.Product)
                    .ThenInclude(p => p.Variants)
                .Include(c => c.Product)
                    .ThenInclude(p => p.Reviews)
                .Include(c => c.Variant) 
                .FirstOrDefaultAsync();

            var cartItemResponseDto = mapper.Map<CartItemResponseDTO>(cartItemWithProduct);

            return new ServiceResponse<CartItemResponseDTO>
            {
                Data = cartItemResponseDto,
                Message = "Cart item added successfully.",
                Success = true
            };
        }



        public async Task<ServiceResponse<CartItemResponseDTO>> UpdateCartItemQuantityAsync(int userId, int cartItemId, int newQuantity)
        {
            var cartItem = await db.CartItems
                .Include(c => c.Product)
                    .ThenInclude(p => p.Variants)
                .Include(c => c.Product)
                    .ThenInclude(p => p.Category)
                .Include(c => c.Product)
                    .ThenInclude(p => p.Reviews)
                .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);

            if (cartItem == null)
            {
                return new ServiceResponse<CartItemResponseDTO>
                {
                    Data = null,
                    Message = "Cart item not found.",
                    Success = false
                };
            }

            var variant = cartItem.Product.Variants.FirstOrDefault(v => v.Id == cartItem.ProductVariantId);

            if (variant == null)
            {
                return new ServiceResponse<CartItemResponseDTO>
                {
                    Data = null,
                    Message = "Product variant not found.",
                    Success = false
                };
            }

            if (variant.Stock < newQuantity)
            {
                return new ServiceResponse<CartItemResponseDTO>
                {
                    Data = null,
                    Message = $"Only {variant.Stock} items in stock for this variant.",
                    Success = false
                };
            }

            cartItem.Quantity = newQuantity;
            db.CartItems.Update(cartItem);
            await db.SaveChangesAsync();

            var updatedDto = mapper.Map<CartItemResponseDTO>(cartItem);

            return new ServiceResponse<CartItemResponseDTO>
            {
                Data = updatedDto,
                Message = "Cart item quantity updated successfully.",
                Success = true
            };
        }




        public async Task<ServiceResponse<List<CartItemResponseDTO>>> RemoveCartItemAsync(int userId, int cartItemId)
        {
            var cartItem = await db.CartItems.FirstOrDefaultAsync(x => x.Id == cartItemId && x.UserId == userId);
            if (cartItem == null)
            {
                return new ServiceResponse<List<CartItemResponseDTO>>
                {
                    Data = null,
                    Message = "Cart Item does not exist",
                    Success = false
                };
            }
            db.CartItems.Remove(cartItem);
            await db.SaveChangesAsync();

            var userCartItems = await db.CartItems
                    .Where(x => x.UserId == userId)
                    .Include(x => x.Product)
                        .ThenInclude(p => p.Category)
                    .Include(x => x.Product)
                        .ThenInclude(p => p.Variants)
                    .Include(x => x.Product)
                        .ThenInclude(p => p.Reviews)
                    .OrderBy(x => x.Added)
                    .ToListAsync();

            var ResponseCartItems = mapper.Map<List<CartItemResponseDTO>>(userCartItems);

            return new ServiceResponse<List<CartItemResponseDTO>>
            {
                Data = ResponseCartItems,
                Message = "Updated cart after item removal",
                Success = true
            };
        }

        public async Task<ServiceResponse<string>> ClearCartAsync(int userId)
        {
            var userCartItems = await db.CartItems.Where(x => x.UserId == userId).ToListAsync();

            if (!userCartItems.Any())
            {
                return new ServiceResponse<string>
                {
                    Data = null,
                    Message = "Cart is already empty.",
                    Success = false
                };
            }

            db.CartItems.RemoveRange(userCartItems);
            await db.SaveChangesAsync();

            return new ServiceResponse<string>
            {
                Data = "Cart cleared successfully.",
                Message = "All items removed from cart.",
                Success = true
            };
        }







    }
}

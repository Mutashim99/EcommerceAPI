using AutoMapper;
using EcommerceAPI.DTOs.ProductDTOs;
using EcommerceAPI.DTOs.UserFavoriteDTOs;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services.UserManagement.UserFavoriteManagement
{
    public class UserFavoriteService : IUserFavorite
    {

        private readonly AppDbContext db;
        private readonly IMapper mapper;
        public UserFavoriteService(AppDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<ServiceResponse<List<FavoriteResponseDTO>>> GetFavoritesAsync(int userId)
        {
            var FavoriteTable = await db.FavoriteItems
                .Where(x => x.UserId == userId)
                .Include(x => x.FavoriteProduct)
                    .ThenInclude(p => p.Category)
                .Include(x => x.FavoriteProduct)
                    .ThenInclude(p => p.Variants)
                .Include(x => x.FavoriteProduct)
                    .ThenInclude(p => p.Reviews)
                .OrderBy(x=> x.DateAdded)
                .ToListAsync();
            
            var FavroiteResponseDTO = mapper.Map<List<FavoriteResponseDTO>>(FavoriteTable);

            return new ServiceResponse<List<FavoriteResponseDTO>>
            {
                Data = FavroiteResponseDTO,
                Message = "Favorite Items Fetched",
                Success = true
            };

           
        }

        public async Task<ServiceResponse<List<ProductResponseDTO>>> AddToFavoritesAsync(int userId, int productId)
        {
            var product = await db.Products.FindAsync(productId);

            if (product == null)
            {
                return new ServiceResponse<List<ProductResponseDTO>>
                {
                    Data = new(),
                    Success = false,
                    Message = "Product not found."
                };
            }

            var alreadyFavorited = await db.FavoriteItems
                .AnyAsync(f => f.UserId == userId && f.ProductId == productId);

            if (alreadyFavorited)
            {
                return new ServiceResponse<List<ProductResponseDTO>>
                {
                    Data = new(),
                    Success = false,
                    Message = "Product already in favorites."
                };
            }

            var favoriteItem = new FavoriteItem
            {
                UserId = userId,
                ProductId = productId,
                DateAdded = DateTime.UtcNow
            };

            db.FavoriteItems.Add(favoriteItem);
            await db.SaveChangesAsync();

            var favoriteItems = await db.FavoriteItems
                .Where(x => x.UserId == userId)
                .Include(x => x.FavoriteProduct)
                    .ThenInclude(p => p.Category)
                .Include(x => x.FavoriteProduct)
                    .ThenInclude(p => p.Variants)
                .Include(x => x.FavoriteProduct)
                    .ThenInclude(p => p.Reviews)
                .OrderBy(x => x.DateAdded)
                .ToListAsync();

            var favoriteResponseDTO = mapper.Map<List<ProductResponseDTO>>(favoriteItems.Select(f => f.FavoriteProduct).ToList());

            return new ServiceResponse<List<ProductResponseDTO>>
            {
                Data = favoriteResponseDTO,
                Success = true,
                Message = "Product added to favorites."
            };
        }

        public async Task<ServiceResponse<List<ProductResponseDTO>>> RemoveFromFavoritesAsync(int userId, int productId)
        {
            var favoriteItem = await db.FavoriteItems
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

            if (favoriteItem == null)
            {
                return new ServiceResponse<List<ProductResponseDTO>>
                {
                    Data = new(),
                    Success = false,
                    Message = "Favorite item not found."
                };
            }

            db.FavoriteItems.Remove(favoriteItem);
            await db.SaveChangesAsync();

            var favoriteItems = await db.FavoriteItems
                .Where(x => x.UserId == userId)
                .Include(x => x.FavoriteProduct)
                    .ThenInclude(p => p.Category)
                .Include(x => x.FavoriteProduct)
                    .ThenInclude(p => p.Variants)
                .Include(x => x.FavoriteProduct)
                    .ThenInclude(p => p.Reviews)
                .OrderBy(x => x.DateAdded)
                .ToListAsync();

            var favoriteResponseDTO = mapper.Map<List<ProductResponseDTO>>(favoriteItems.Select(f => f.FavoriteProduct).ToList());

            return new ServiceResponse<List<ProductResponseDTO>>
            {
                Data = favoriteResponseDTO,
                Success = true,
                Message = "Product removed from favorites."
            };
        }

    }
}

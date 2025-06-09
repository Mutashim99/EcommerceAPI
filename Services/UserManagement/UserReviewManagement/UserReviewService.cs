using AutoMapper;
using EcommerceAPI.DTOs.ProductDTOs;
using EcommerceAPI.DTOs.ReviewDTOs;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services.UserManagement.UserReviewManagement
{
    public class UserReviewService : IUserReview
    {
        private readonly AppDbContext db;
        private readonly IMapper mapper;
        public UserReviewService(AppDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
        public async Task<ServiceResponse<List<UserReviewResponseDTO>>> GetMyReviewsAsync(int userId)
        {
            var reviewWithProductAccordingToUser = await db.Reviews.Include(x=> x.Product).Where(x=>x.UserId == userId).ToListAsync();

            var userReviewsDTOMapped = mapper.Map<List<UserReviewResponseDTO>>(reviewWithProductAccordingToUser);
            return new ServiceResponse<List<UserReviewResponseDTO>> {
                Data = userReviewsDTOMapped ,
                Message = "Reviews fetched succesfully",
                Success = true
            };
        }

        public async Task<ServiceResponse<UserReviewResponseDTO>> GetMyReviewForProductAsync(int userId, int productId)
        {
            var reviewWithProduct = await db.Reviews.Include(x => x.Product).FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);
            if (reviewWithProduct == null)
            {
                return new ServiceResponse<UserReviewResponseDTO>
                {
                    Success = false,
                    Message = "Review not found.",
                    Data = null
                };
            }

            var userReviewResponseDTOMapped = mapper.Map<UserReviewResponseDTO>(reviewWithProduct);

            return new ServiceResponse<UserReviewResponseDTO>
            {
                Success = true,
                Message = "Review retrieved successfully.",
                Data = userReviewResponseDTOMapped
            };
        }

        public async Task<ServiceResponse<UserReviewResponseDTO>> AddReviewAsync(int userId, CreateReviewDTO reviewDto)
        {
            if(reviewDto == null)
            {
                return new ServiceResponse<UserReviewResponseDTO>
                {
                    Data = null,
                    Success = false,
                    Message = "Incorrect or Incomplete Data to Add a review"
                };
            }
            var productExists = await db.Products.AnyAsync(p => p.Id == reviewDto.ProductId);
            if (!productExists)
            {
                return new ServiceResponse<UserReviewResponseDTO>
                {
                    Data = null,
                    Success = false,
                    Message = "Invalid product ID. Product does not exist."
                };
            }

            var dtoToRealReviewConverted = mapper.Map<Review>(reviewDto);
            dtoToRealReviewConverted.UserId = userId;

            await db.Reviews.AddAsync(dtoToRealReviewConverted);
            await db.SaveChangesAsync();

            var reviewForResponse = await db.Reviews.Include(x=> x.Product).FirstOrDefaultAsync(x=> x.Id == dtoToRealReviewConverted.Id);

            var mappedtoResponseDTO = mapper.Map<UserReviewResponseDTO>(reviewForResponse);

            return new ServiceResponse<UserReviewResponseDTO>
            {
                Success = true,
                Data = mappedtoResponseDTO,
                Message = "Review Added Succesfully"
            };
        }

        public async Task<ServiceResponse<UserReviewResponseDTO>> UpdateReviewAsync(int userId, int reviewId, UpdateReviewDTO reviewDto)
        {
            if (reviewDto == null)
            {
                return new ServiceResponse<UserReviewResponseDTO>
                {
                    Data = null,
                    Success = false,
                    Message = "Invalid update data."
                };
            }

            var existingReview = await db.Reviews.Include(r => r.Product)
                                                 .FirstOrDefaultAsync(r => r.Id == reviewId && r.UserId == userId);

            if (existingReview == null)
            {
                return new ServiceResponse<UserReviewResponseDTO>
                {
                    Data = null,
                    Success = false,
                    Message = "Review not found or access denied."
                };
            }

            existingReview.Rating = reviewDto.Rating;
            existingReview.Comment = reviewDto.Comment;

            await db.SaveChangesAsync();

            var updatedDto = mapper.Map<UserReviewResponseDTO>(existingReview);

            return new ServiceResponse<UserReviewResponseDTO>
            {
                Success = true,
                Data = updatedDto,
                Message = "Review updated successfully."
            };
        }


        public async Task<ServiceResponse<string>> DeleteReviewAsync(int userId, int reviewId)
        {
            var review = await db.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId && r.UserId == userId);

            if (review == null)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Data = null,
                    Message = "Review not found or access denied."
                };
            }

            db.Reviews.Remove(review);
            await db.SaveChangesAsync();

            return new ServiceResponse<string>
            {
                Success = true,
                Data = "Review deleted successfully.",
                Message = "Review deleted successfully."
            };
        }

        public async Task<ServiceResponse<List<PendingReviewProductDTO>>> GetReviewableProductsAsync(int userId)
        {
            var reviewedProductIds = await db.Reviews
                .Where(r => r.UserId == userId)
                .Select(r => r.ProductId)
                .ToListAsync();

            var pendingReviewItems = await db.OrderItems
                .Where(oi => oi.Order.UserId == userId && !reviewedProductIds.Contains(oi.ProductId))
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .ToListAsync();

            var dtoList = mapper.Map<List<PendingReviewProductDTO>>(pendingReviewItems);

            return new ServiceResponse<List<PendingReviewProductDTO>>
            {
                Data = dtoList,
                Success = true,
                Message = "Pending review products fetched successfully."
            };
        }



    }
}

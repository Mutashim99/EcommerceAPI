using AutoMapper;
using EcommerceAPI.Data;
using EcommerceAPI.DTOs.AdminDTOs;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services.AdminManagement.AdminReview
{
    public class AdminReviewService : IAdminReview
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AdminReviewService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<AdminReviewResponseDTO>>> GetAllReviewsAsync()
        {
            var reviewEntities = await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Product)
                    .ThenInclude(p => p.Category)
                .ToListAsync();

            var reviewDTOs = _mapper.Map<List<AdminReviewResponseDTO>>(reviewEntities);

            return new ServiceResponse<List<AdminReviewResponseDTO>>
            {
                Data = reviewDTOs,
                Success = true,
                Message = "All reviews retrieved successfully."
            };
        }

        public async Task<ServiceResponse<List<AdminReviewResponseDTO>>> FilterReviewsbyProductAsync(int productId)
        {
            var reviewEntities = await _context.Reviews
                .Where(r => r.ProductId == productId)
                .Include(r => r.User)
                .Include(r => r.Product)
                    .ThenInclude(p => p.Category)
                .ToListAsync();

            var reviewDTOs = _mapper.Map<List<AdminReviewResponseDTO>>(reviewEntities);

            return new ServiceResponse<List<AdminReviewResponseDTO>>
            {
                Data = reviewDTOs,
                Success = true,
                Message = $"Reviews for product ID {productId} retrieved successfully."
            };
        }

        public async Task<ServiceResponse<string>> DeleteReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Review not found."
                };
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return new ServiceResponse<string>
            {
                Data = $"Review ID {reviewId} deleted successfully.",
                Success = true
            };
        }
    }
}

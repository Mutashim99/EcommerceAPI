using EcommerceAPI.DTOs.ProductDTOs;
using EcommerceAPI.DTOs.ReviewDTOs;

namespace EcommerceAPI.Services.UserManagement.UserReviewManagement
{
    public interface IUserReview
    {
        // Get all reviews created by the logged-in user
        Task<ServiceResponse<List<ReviewResponseDTO>>> GetMyReviewsAsync(int userId);

        // Get a single review for a specific product created by the user
        Task<ServiceResponse<ReviewResponseDTO>> GetMyReviewForProductAsync(int userId, int productId);

        // Add a new review (only if purchased and not already reviewed)
        Task<ServiceResponse<ReviewResponseDTO>> AddReviewAsync(int userId, ReviewCreateDTO reviewDto);

        // Update an existing review created by the user
        Task<ServiceResponse<ReviewResponseDTO>> UpdateReviewAsync(int userId, int reviewId, ReviewUpdateDTO reviewDto);

        // Delete a review written by the user
        Task<ServiceResponse<string>> DeleteReviewAsync(int userId, int reviewId);

        // Get all products the user has purchased but not reviewed yet
        Task<ServiceResponse<List<ProductResponseDTO>>> GetReviewableProductsAsync(int userId);
    }
}

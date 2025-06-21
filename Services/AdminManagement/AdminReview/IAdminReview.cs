using EcommerceAPI.DTOs.AdminDTOs;

namespace EcommerceAPI.Services.AdminManagement.AdminReview
{
    public interface IAdminReview
    {
        Task<ServiceResponse<List<AdminReviewResponseDTO>>> GetAllReviewsAsync();
        Task<ServiceResponse<List<AdminReviewResponseDTO>>> FilterReviewsbyProductAsync(int productId);
        Task<ServiceResponse<string>> DeleteReviewAsync(int reviewId);

    }
}

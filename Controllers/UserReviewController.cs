using EcommerceAPI.DTOs.ReviewDTOs;
using EcommerceAPI.Services.UserManagement.UserReviewManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserReviewController : ControllerBase
    {
        private readonly IUserReview userReview;

        public UserReviewController(IUserReview userReview)
        {
            this.userReview = userReview;
        }

        [HttpGet("GetMyReviews")]
        [Authorize]
        public async Task<ActionResult<List<UserReviewResponseDTO>>> GetMyReviewsAsync()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }
            var GetUserReviewResult = await userReview.GetMyReviewsAsync(userId);

            return Ok(GetUserReviewResult.Data);
        }
        [HttpGet("GetMyReviewForProduct/{productId}")]
        [Authorize]
        public async Task<ActionResult<UserReviewResponseDTO>> GetMyReviewForProductAsync(int productId)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }
            var GetMyReviewResult = await userReview.GetMyReviewForProductAsync(userId, productId);
            if(GetMyReviewResult.Success == false) {
                return BadRequest(GetMyReviewResult.Message);    
            }

            return Ok(GetMyReviewResult.Data);
        }

        [HttpPost("AddReview")]
        [Authorize]
        public async Task<ActionResult<UserReviewResponseDTO>> AddReviewAsync(CreateReviewDTO reviewDto)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }

            var AddReviewResult = await userReview.AddReviewAsync(userId, reviewDto);
            if (AddReviewResult.Success == false)
            {
                return BadRequest(AddReviewResult.Message);
            }
            return Ok(AddReviewResult.Data);
        }

        [HttpPut("UpadateReview/{reviewId}")]
        [Authorize]
        public async Task<ActionResult<UserReviewResponseDTO>> UpdateReviewAsync (int reviewId, UpdateReviewDTO reviewDto)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }
            var UpdateReviewResult = await  userReview.UpdateReviewAsync(userId,reviewId,reviewDto);
            if (UpdateReviewResult.Success == false)
            {
                return BadRequest(UpdateReviewResult.Message);
            }
            return Ok(UpdateReviewResult.Data);
        }

        [HttpDelete("DeleteReview/{reviewId}")]
        [Authorize]
        public async Task<ActionResult<string>> DeleteReviewAsync(int reviewId)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }
            var DeleteReviewResult = await userReview.DeleteReviewAsync(userId,reviewId);
            if (DeleteReviewResult.Success == false)
            {
                return BadRequest(DeleteReviewResult.Message);
            }
            return Ok(DeleteReviewResult.Data);

        }
        [HttpGet("GetReviewableProducts")]
        [Authorize]
        public async Task<ActionResult<List<PendingReviewProductDTO>>> GetReviewableProductsAsync()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }
            var GetReviewableProductResult = await userReview.GetReviewableProductsAsync(userId);
            if(GetReviewableProductResult.Success == false)
            {
                return BadRequest(GetReviewableProductResult.Message);
            }    
            return Ok(GetReviewableProductResult.Data);
        }
    }
}

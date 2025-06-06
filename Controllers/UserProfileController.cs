using EcommerceAPI.DTOs.UserProfileDTOs;
using EcommerceAPI.Services.UserManagement.UserProfileManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfile profileService;
        public UserProfileController(IUserProfile profileService)
        {
            this.profileService = profileService;   
        }
        [Authorize]
        [HttpGet("GetUserProfile")]
        public async Task<ActionResult<UserProfileDTO>> GetUserProfileAsync()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }
            var getuserprofileresponse = await profileService.GetUserProfileAsync(userId);
            if(getuserprofileresponse.Success == false)
            {
                return NotFound(getuserprofileresponse.Message);
            }
            return Ok(getuserprofileresponse.Data);
        }
        [Authorize]
        [HttpPost("UpdateUsername")]
        public async Task<ActionResult<UserProfileDTO>> UpdateUserNameAsync(UpdateUsernameDTO usernameDTO)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }
            var getUserNameResponse = await profileService.UpdateUserNameAsync(userId,usernameDTO.NewName);
            if (getUserNameResponse.Success == false)
            {
                return NotFound(getUserNameResponse.Message);
            }
            return Ok(getUserNameResponse.Data);
        }
        [Authorize]
        [HttpPost("UpdatePhoneNumber")]
        public async Task<ActionResult<UserProfileDTO>> UpdatePhoneNumberAsync(UpdatePhoneNumberDTO phoneNumberDTO)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }
            var getPhoneResponse = await profileService.UpdatePhoneNumberAsync(userId, phoneNumberDTO.NewPhoneNumber);
            if (getPhoneResponse.Success == false)
            {
                return NotFound(getPhoneResponse.Message);
            }

            return Ok(getPhoneResponse.Data);
        }
        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult<string>> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }
            var getPhoneResponse = await profileService.ChangePasswordAsync(userId,changePasswordDTO.OldPassword, changePasswordDTO.NewPassword);
            if (getPhoneResponse.Success == false)
            {
                return NotFound(getPhoneResponse.Message);
            }

            return Ok(getPhoneResponse.Message);

        }

        [HttpPost("ForgotPassword")]
        public async Task<ActionResult<string>> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO)
        {
            var getForgotPasswordResponse = await profileService.ForgotPasswordAsync(forgotPasswordDTO.Email);
            if(getForgotPasswordResponse.Success == false)
            {
                return BadRequest(getForgotPasswordResponse.Message);
            }
            return Ok(getForgotPasswordResponse.Message);
        }
        [HttpPost("ResetPassword")]
        public async Task<ActionResult<string>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            var getResetPasswordResponse = await profileService.ResetPasswordAsync(resetPasswordDTO.Token, resetPasswordDTO.NewPassword);
            if (getResetPasswordResponse.Success == false)
            {
                return Unauthorized(getResetPasswordResponse.Message);
            }
            return Ok(getResetPasswordResponse.Message);
        }
    }
}

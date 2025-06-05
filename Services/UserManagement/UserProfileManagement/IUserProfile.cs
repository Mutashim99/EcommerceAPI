using EcommerceAPI.DTOs;
using EcommerceAPI.Models;

namespace EcommerceAPI.Services.UserManagement.UserProfileManagement
{
    public interface IUserProfile
    {
        public Task<ServiceResponse<UserProfileDTO>> GetUserProfileAsync(int Id); //get user id from JWT
        public Task<ServiceResponse<UserProfileDTO>> UpdateUserNameAsync(int Id, string newName); //get user id from JWT
        public Task<ServiceResponse<UserProfileDTO>> UpdatePhoneNumberAsync(int Id, string newPhoneNumber); //get user id from JWT
        public Task<ServiceResponse<string>> ChangePasswordAsync(int Id, string oldPassword, string newPassword); //get user id from JWT
        public Task<ServiceResponse<string>> ForgotPasswordAsync(string email); // we will send an email with the link to the frontend page where user can enter their new password and with the ResetPasswordToken that will be used in ResetPassword endpoint to find the user by matching the ResetPasswordToken and then updating the given password with hashing and making th ResetPasswordToken= null 
        public Task<ServiceResponse<string>> ResetPasswordAsync(string token,string newpassword);







    }
}

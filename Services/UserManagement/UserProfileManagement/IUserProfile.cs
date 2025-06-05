using EcommerceAPI.DTOs;

namespace EcommerceAPI.Services.UserManagement.UserProfileManagement
{
    public interface IUserProfile
    {
        public Task<ServiceResponse<UserProfileDTO>> UserProfileAsync(); //get user id from JWT
        public Task<ServiceResponse<string>> UpdateUserNameAsync(string newName); //get user id from JWT
        public Task<ServiceResponse<string>> UpdatePhoneNumberAsync(string newPhoneNumber); //get user id from JWT
        public Task<ServiceResponse<string>> ChangePasswordAsync(string oldPassword, string newPassword); //get user id from JWT









    }
}

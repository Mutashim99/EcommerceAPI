
namespace EcommerceAPI.Services.UserManagement.UserProfileManagement
{
    public class UserProfileService : IUserProfile
    {
        public Task<ServiceResponse<string>> ChangePasswordAsync(string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<string>> UpdatePhoneNumberAsync(string newPhoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<string>> UpdateUserNameAsync(string newName)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<UserProfileDTO>> UserProfileAsync()
        {
            throw new NotImplementedException();
        }
    }
}

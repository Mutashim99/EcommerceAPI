using EcommerceAPI.DTOs.AuthDTOs;
using EcommerceAPI.Models;

namespace EcommerceAPI.Services.Auth
{
    public interface IAuth
    {
        public Task<ServiceResponse<string>> RegisterAsync(UserRegistrationDTO user);
        public Task<ServiceResponse<string>> LoginAsync(UserLoginDTO user);
        public string CreateToken(User user);
        public Task<ServiceResponse<string>> VerifyEmailAsync(string token); //for the verification of email at the time of signup and the token in this isnt the JWT its the token generated that will be matched with the link sent to the emil for verification
        // this will be used in /verify-email endpoint

    }
}

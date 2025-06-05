
using AutoMapper;
using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services.UserManagement.UserProfileManagement
{
    public class UserProfileService : IUserProfile
    {
        private readonly AppDbContext db;
        private readonly IMapper mapper;


        public UserProfileService(AppDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<ServiceResponse<UserProfileDTO>> GetUserProfileAsync(int Id)
        {
            
            var CurrentUser = await db.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (CurrentUser == null){
                return new ServiceResponse<UserProfileDTO>
                {
                    Data = null,
                    Message = "User not found",
                    Success = false
                };
            }

            
          var userProfileDTO = mapper.Map<UserProfileDTO>(CurrentUser);


            return new ServiceResponse<UserProfileDTO>
            {
                Data = userProfileDTO,
                Message = "UserProfile Fetched Succesfully",
                Success = true
            };
        }

        public async Task<ServiceResponse<UserProfileDTO>> UpdateUserNameAsync(int Id, string newName)
        {
            var CurrentUser = await db.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if( CurrentUser != null) { 
                CurrentUser.Username = newName;

                var userProfileDTO = mapper.Map<UserProfileDTO>(CurrentUser);

                return new ServiceResponse<UserProfileDTO>
                {
                    Data = userProfileDTO,
                    Message = "UserName Updated Succesfully",
                    Success = true
                };
            }
            return new ServiceResponse<UserProfileDTO>
            {
                Data = null,
                Message = "User not found",
                Success = false
            };

        }
        public async Task<ServiceResponse<UserProfileDTO>> UpdatePhoneNumberAsync(int Id, string newPhoneNumber)
        {
            var CurrentUser = await db.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (CurrentUser != null)
            {
                CurrentUser.PhoneNumber = newPhoneNumber;

                var userProfileDTO = mapper.Map<UserProfileDTO>(CurrentUser);

                return new ServiceResponse<UserProfileDTO>
                {
                    Data = userProfileDTO,
                    Message = "Phone Number Updated Succesfully",
                    Success = true
                };
            }
            return new ServiceResponse<UserProfileDTO>
            {
                Data = null,
                Message = "User not found",
                Success = false
            };
        }

        public async Task<ServiceResponse<string>> ChangePasswordAsync(int Id, string oldPassword, string newPassword)
        {
            var CurrentUser = await db.Users.FirstOrDefaultAsync(x => x.Id == Id);

            if (CurrentUser == null)
            {
                return new ServiceResponse<string>
                {
                    Data = null,
                    Message = "User not found",
                    Success = false
                };
            }
            if (!BCrypt.Net.BCrypt.Verify(oldPassword, CurrentUser.PasswordHash))
            {
                return new ServiceResponse<string>
                {
                    Data = null,
                    Message = "Old Passowrd didnt match",
                    Success = false
                };
            }
           

            var newHashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
            CurrentUser.PasswordHash = newHashedPassword;



            return new ServiceResponse<string>
            {
                Data = null,
                Message = "Password Updated Succesfully",
                Success = true
            };
        }

        public Task<ServiceResponse<string>> ForgotPasswordAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<string>> ResetPasswordAsync(string token, string newpassword)
        {
            throw new NotImplementedException();
        }

 

        

       

      
    }
}


using AutoMapper;
using EcommerceAPI.DTOs.UserProfileDTOs;
using EcommerceAPI.Models;
using EcommerceAPI.Services.Email;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services.UserManagement.UserProfileManagement
{
    public class UserProfileService : IUserProfile
    {
        private readonly AppDbContext db;
        private readonly IMapper mapper;
        private readonly IConfiguration _config;
        private readonly IEmail sendemail;

        public UserProfileService(AppDbContext db, IMapper mapper , IConfiguration config, IEmail email)
        {
            this.db = db;
            this.mapper = mapper;
            _config = config;
            sendemail = email;
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
                await db.SaveChangesAsync();
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
                await db.SaveChangesAsync();
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
            await db.SaveChangesAsync();


            return new ServiceResponse<string>
            {
                Data = null,
                Message = "Password Updated Succesfully",
                Success = true
            };
        }

        public async Task<ServiceResponse<string>> ForgotPasswordAsync(string email)
        {
            if(email == null)
            {
                return new ServiceResponse<string>
                {
                    Data = null,
                    Message = "please provide an email",
                    Success = false
                };

            }
            var CurrentUser = await db.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (CurrentUser == null)
            {
                return new ServiceResponse<string>
                {
                    Data = null,
                    Message = "Can not find any user with this email",
                    Success = false
                };
            }
            var Resettoken = Guid.NewGuid().ToString();
            CurrentUser.ResetPasswordToken = Resettoken;
            CurrentUser.ResetPasswordTokenExpiry = DateTime.UtcNow.AddMinutes(30);
            await db.SaveChangesAsync();

            var Passresetlink = $"{_config["FrontendDomainForEmailVerification"]}/reset-password/{CurrentUser.ResetPasswordToken}";

            var subject = "Password Reset Link";
            var body = $@"
                    <h1>Reset Your Password</h1>
                    <h2>here is the link to reset your password! 
                        <a href='{Passresetlink}' target='_blank'>this link</a>.
                    </h2>";
            var SendEmailResult = await sendemail.SendEmailAsync(CurrentUser.Email, subject, body);
            if (!SendEmailResult)
            {
                return new ServiceResponse<string>
                {
                    Data = null,
                    Message = "Can not send you an email right now! try again later",
                    Success = false
                };
            }
            return new ServiceResponse<string>
            {
                Data = null,
                Message = "Password Reset Link Sent Succesfully",
                Success = true
            };
        }

        public async Task<ServiceResponse<string>> ResetPasswordAsync(string token, string newpassword)
        {
            var Currentuser = await db.Users.FirstOrDefaultAsync(u => u.ResetPasswordToken == token);

            if (Currentuser == null || Currentuser.ResetPasswordTokenExpiry < DateTime.UtcNow)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Invalid or expired password reset token.",
                    Data = null
                };
            }

            var newHashedPassword = BCrypt.Net.BCrypt.HashPassword(newpassword);
            Currentuser.PasswordHash = newHashedPassword;
            await db.SaveChangesAsync();
            Currentuser.ResetPasswordToken = null;
            Currentuser.ResetPasswordTokenExpiry = null;


            return new ServiceResponse<string>
            {
                Data = null,
                Message = "Password Updated Succesfully",
                Success = true
            };
        }


    }
}

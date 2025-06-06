using AutoMapper;
using BCrypt.Net;
using EcommerceAPI.Data;
using EcommerceAPI.DTOs.AuthDTOs;
using EcommerceAPI.Models;
using EcommerceAPI.Services.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcommerceAPI.Services.Auth
{
    public class AuthService : IAuth
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext db;
        private readonly IMapper mapper;
        private readonly IEmail sendemail;

        public AuthService(IConfiguration _config , AppDbContext db , IMapper mapper , IEmail sendemail)
        {
            this._config = _config;
            this.db = db;
            this.mapper = mapper;
            this.sendemail = sendemail;
        }
        public string CreateToken(User user)
        {
            List<Claim> claims = new() // these are the credentials that can be stored in jwt and access anytime(jaise session mein krta tha)
        { //Roles are also checked using these claims for [authorize(Role="admin")]
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("UserId", user.Id.ToString()) // ✅ User ID claim
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ServiceResponse<string>>  RegisterAsync(UserRegistrationDTO user)
        {
            var CheckExistingUser = await db.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (CheckExistingUser != null) {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Email already exist.",
                    Data = null
                };
            }
            var RealUser = mapper.Map<User>(user);

            RealUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            RealUser.EmailVerificationToken = Guid.NewGuid().ToString();

            var VerificationUrl = $"{_config["FrontendDomainForEmailVerification"]}/verify-token/{RealUser.EmailVerificationToken}";

            //generate the verification url and send it via calling the sendemail method in emailservice
            var subject = "EMAIL VERIFICATION LINK";
            var body = $@"
                    <h1>Thank you for signing up!</h1>
                    <h2>You can verify your email using 
                        <a href='{VerificationUrl}' target='_blank'>this link</a>.
                    </h2>";

            var emailsendresult = await sendemail.SendEmailAsync(RealUser.Email, subject, body);
            if (emailsendresult == false) {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "There was an error sending you verification Email, try again later or with another email",
                    Data = null
                };
            }

            db.Users.Add(RealUser);
            await db.SaveChangesAsync();

            return new ServiceResponse<string>
            {
                Success = true,
                Message = "Registration successful. Please check your email to verify your account.",
                Data = null
            };

        }


        public async Task<ServiceResponse<string>> LoginAsync(UserLoginDTO user)
        {
            if (user == null)
            {
                return new ServiceResponse<string>
                {
                    Data = null,
                    Success = false,
                    Message = "Enter all the information first"
                };
            }

            
            var CurrentUser = await db.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (CurrentUser == null)
            {
                return new ServiceResponse<string>
                {
                    Data = null,
                    Success = false,
                    Message = "User not found"
                };
            }
            if (CurrentUser.Email != user.Email || !BCrypt.Net.BCrypt.Verify(user.Password, CurrentUser.PasswordHash))
            {
                return new ServiceResponse<string>
                {
                    Data = null,
                    Success = false,
                    Message = "Wrong credentials"
                };
            }
            if (!CurrentUser.IsEmailVerified)
            {
                return new ServiceResponse<string>
                {
                    Data = null,
                    Success = false,
                    Message = "Please verify your email before logging in."
                };
            }
            var token = CreateToken(CurrentUser);

            return new ServiceResponse<string>
            {
                Data = token,
                Success = true,
                Message = "Succesfully logged In!"
            };
        }



        public async Task<ServiceResponse<string>> VerifyEmailAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new ServiceResponse<string>
                {
                    Message = "Invalid verification token.",
                    Data = null,
                    Success = false,
                };
            }
              var tokenOwner = await db.Users.FirstOrDefaultAsync(x => x.EmailVerificationToken == token);

            if (tokenOwner == null)
            {
                    return new ServiceResponse<string>
                    {
                        Message = "Invalid or expired verification token.",
                        Data = null,
                        Success = false,
                    }; 
            }

            if (tokenOwner.IsEmailVerified)
            {
                    return new ServiceResponse<string>
                    {
                        Message = "Email is already verified.",
                        Data = null,
                        Success = true,
                    }; 
            }
            


            tokenOwner.IsEmailVerified = true;
            tokenOwner.EmailVerificationToken = null; 

            await db.SaveChangesAsync();

                return new ServiceResponse<string>
                {
                    Message = "Email verified Succesfully.",
                    Data = null,
                    Success = true
                };
            }

    }
}

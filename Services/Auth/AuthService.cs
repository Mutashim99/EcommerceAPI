using AutoMapper;
using BCrypt.Net;
using EcommerceAPI.Data;
using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        public AuthService(IConfiguration _config , AppDbContext db , IMapper mapper)
        {
            this._config = _config;
            this.db = db;
            this.mapper = mapper;
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

            db.Users.Add( RealUser );
            await db.SaveChangesAsync();

            //generate the verification url and send it via calling the sendemail method in emailservice

            return new ServiceResponse<string>
            {
                Success = true,
                Message = "Registration successful. Please check your email to verify your account.",
                Data = null
            };


            

        }


        public Task<ServiceResponse<string>> LoginAsync(UserLoginDTO user)
        {
            throw new NotImplementedException();
        }

       

        public Task<bool> VerifyEmailAsync(string token)
        {
            throw new NotImplementedException();
        }
    }
}

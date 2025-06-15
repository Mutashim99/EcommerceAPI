
using AutoMapper;
using EcommerceAPI.DTOs.AdminDTOs;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services.AdminManagement.AdminAuth
{
    public class AdminAuthService : IAdminAuth
    {
        private readonly AppDbContext db;
        private readonly IMapper mapper;

        public AdminAuthService(IMapper mapper , AppDbContext db)
        {
            this.mapper = mapper;
            this.db = db;
        }

        public async Task<ServiceResponse<string>> registerAdmin(RegisterAdminDTO registerAdminDTO)
        {


            var currentAdmin = await db.Users.FirstOrDefaultAsync(x => x.Email == registerAdminDTO.Email);
            if (currentAdmin != null)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Admin already exist.",
                    Data = null
                };
            }
            var MappedAdmin = mapper.Map<User>(registerAdminDTO);
            MappedAdmin.Role = "Admin";
            MappedAdmin.IsEmailVerified = true;
            MappedAdmin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerAdminDTO.Password);
            await db.Users.AddAsync(MappedAdmin);
            await db.SaveChangesAsync();

            return new ServiceResponse<string>
            {
                Success = true,
                Message = "Admin Registered Succesfully",
                Data = $"Admin {MappedAdmin.Username} with Email {MappedAdmin.Email} has been Registered Succesfully"
            };
        }
    }
}

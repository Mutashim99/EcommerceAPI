using EcommerceAPI.DTOs.AdminDTOs;

namespace EcommerceAPI.Services.AdminManagement.AdminAuth
{
    public interface IAdminAuth
    {
        public Task<ServiceResponse<string>> registerAdmin(RegisterAdminDTO registerAdminDTO);
    }
}

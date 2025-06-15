using EcommerceAPI.DTOs.AdminDTOs;
using EcommerceAPI.Services.AdminManagement.AdminAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRegisterController : ControllerBase
    {
        private readonly IAdminAuth adminAuth;

        public AdminRegisterController(IAdminAuth adminAuth)
        {
            this.adminAuth = adminAuth;
        }

        [HttpGet]
        [Authorize(Roles ="SuperAdmin")]
        public async Task<IActionResult> registerAdmin(RegisterAdminDTO registerAdminDTO)
        {
            var registerAdminResult = await adminAuth.registerAdmin(registerAdminDTO);
            if (registerAdminResult.Success == true)
            {
                return Ok(new { message = registerAdminResult.Message, success = registerAdminResult.Success });
            }
            return BadRequest(new { message = registerAdminResult.Message, success = registerAdminResult.Success });
        }
    }
}

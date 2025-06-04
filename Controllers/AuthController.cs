using EcommerceAPI.DTOs;
using EcommerceAPI.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth auth;

        public AuthController(IAuth auth)
        {
            this.auth = auth;
        }

        [HttpPost("/signup")]
        public async Task<ActionResult> SignUp(UserRegistrationDTO registerdto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var RegisterAsyncResult = await auth.RegisterAsync(registerdto);
            if(RegisterAsyncResult.Success == true)
            {
                return Ok(RegisterAsyncResult.Message);
            }
            return BadRequest(RegisterAsyncResult.Message);
        }

        [HttpPost("/login")]
        public async Task<ActionResult<string>> Login(UserLoginDTO logindto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var LoginResponse = await auth.LoginAsync(logindto);
            if (LoginResponse.Success == true)
            {
                return Ok(new
                {
                    token = LoginResponse.Data,
                    message = LoginResponse.Message
                });
            }
            return BadRequest(LoginResponse.Message);

        }
        [HttpGet("/verify-email")]
        public async Task<ActionResult> VerifyEmail([FromQuery] string token)
        {
            var verifymethodcall = await auth.VerifyEmailAsync(token);
           if(verifymethodcall.Success == true)
                return Ok(verifymethodcall.Message);

           return BadRequest(verifymethodcall.Message);
        }
    }
}

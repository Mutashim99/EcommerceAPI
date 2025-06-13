using EcommerceAPI.DTOs.AuthDTOs;
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

        [HttpPost("Signup")]
        public async Task<ActionResult> SignUp(UserRegistrationDTO registerdto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var RegisterAsyncResult = await auth.RegisterAsync(registerdto);
            if(RegisterAsyncResult.Success == true)
            {
                return Ok(new {message = RegisterAsyncResult.Message , success = RegisterAsyncResult.Success });
            }
            return BadRequest(new { message =  RegisterAsyncResult.Message, success = RegisterAsyncResult.Success });
        }

        [HttpPost("Login")]
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
                    message = LoginResponse.Message,
                    success = LoginResponse.Success
                });
            }
            return BadRequest( new { LoginResponse.Message , success = LoginResponse.Success });

        }
        [HttpPost("VerifyEmail")]
        public async Task<ActionResult> VerifyEmail(VerifyEmailDTO verifyEmailDTO)
        {
            var verifymethodcall = await auth.VerifyEmailAsync(verifyEmailDTO.Token);
           if(verifymethodcall.Success == true)
                return Ok(verifymethodcall.Message);

           return BadRequest(verifymethodcall.Message);
        }
    }
}

using EcommerceAPI.DTOs.UserAddressDTOs;
using EcommerceAPI.Services.UserManagement.UserAddressManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAddressController : ControllerBase
    {
        private readonly IUserAddress userAddressService;

        public UserAddressController(IUserAddress userAddressService)
        {
            this.userAddressService = userAddressService;
        }

        [Authorize]
        [HttpGet("GetAddresses")]
        public async Task<ActionResult<List<AddressResponseDto>>> GetAddressesAsync()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }

            var getUserAddressesResult  = await userAddressService.GetUserAddressesAsync(userId);
            
            return Ok(getUserAddressesResult.Data);
        }

        [Authorize]
        [HttpPost("AddAddress")]
        public async Task<ActionResult<AddressResponseDto>> AddAddressAsync(CreateAddressDto createAddressDto)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }

            if (createAddressDto == null)
            {
                return BadRequest("Wrong Address Details");
            }
            var addAddressResult = await userAddressService.AddAddressAsync(userId , createAddressDto);

            if(addAddressResult.Success == false)
            {
                return BadRequest(addAddressResult.Message);
            }
            return Ok(addAddressResult.Data);
        }

        [Authorize]
        [HttpPut("UpdateAddress/{AddressId}")]
        public async Task<ActionResult<AddressResponseDto>> UpdateAddressAsync(int AddressId , CreateAddressDto updateAddressDto)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }
            if(updateAddressDto == null)
            {
                return BadRequest("Wrong Updated Address");
            }
           
            var updateAddressResult = await  userAddressService.UpdateAddressAsync(userId, AddressId , updateAddressDto);

            if(updateAddressResult.Success == false)
            {
                return BadRequest(updateAddressResult.Message);
            }
            return Ok(updateAddressResult.Data);
        }

        [Authorize]
        [HttpDelete("DeleteAddress/{AddressId}")]
        public async Task<ActionResult<string>> DeleteAddressAsync(int AddressId)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }
            var DeleteAddressResult = await userAddressService.DeleteAddressAsync(userId, AddressId);
            if (DeleteAddressResult.Success == false)
            {
                return BadRequest(DeleteAddressResult.Message);
            }
            return Ok(DeleteAddressResult.Data);
        }
    }
}

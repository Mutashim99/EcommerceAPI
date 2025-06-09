using EcommerceAPI.DTOs.UserCartDTOs;
using EcommerceAPI.Services.UserManagement.UserCartManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCartController : ControllerBase
    {
        private readonly IUserCart userCart;
        public UserCartController(IUserCart userCart)
        {
            this.userCart = userCart;
        }
        [Authorize]
        [HttpGet("GetCartItems")]
        public async Task<ActionResult<List<CartItemResponseDTO>>> GetCartItemsAsync()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }
            var getCartItemResponse = await userCart.GetCartItemsAsync(userId);

            return Ok(getCartItemResponse.Data);
        }

        [Authorize]
        [HttpPost("AddCartItem")]
        public async Task<ActionResult<CartItemResponseDTO>> AddCartItemAsync(CreateCartItemDTO request)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }
            var addCartItemResponse = await userCart.AddCartItemAsync(userId, request);
            if (addCartItemResponse.Success == false)
            {
                return BadRequest(addCartItemResponse.Message);
            }

            return Ok(addCartItemResponse.Data);
        }

        [Authorize]
        [HttpPut("UpdateCartItemQuantity")]
        public async Task<ActionResult<CartItemResponseDTO>> UpdateCartItemQuantityAsync(UpdateCartItemQuantityDTO request)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }
            if(request == null)
            {
                return BadRequest("Quantity and CartItemId is required");
            }

            var UpdateCartQuantityResponse = await userCart.UpdateCartItemQuantityAsync(userId, request.CartItemId, request.NewQuantity);
            if(UpdateCartQuantityResponse.Success == false)
            {
                return BadRequest(UpdateCartQuantityResponse.Message);
            }

            return Ok(UpdateCartQuantityResponse.Data);
        }

        [Authorize]
        [HttpDelete("RemoveCartItem/{cartItemId}")]
        public async Task<ActionResult<List<CartItemResponseDTO>>> RemoveCartItemAsync(int cartItemId)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }

            if (cartItemId == null)
            {
                return BadRequest("CartItem Id is required");
            }

            var RemoveCartItemResponse = await userCart.RemoveCartItemAsync(userId, cartItemId);
            if (RemoveCartItemResponse.Success == false)
            {
                return BadRequest(RemoveCartItemResponse.Message);
            }
            return Ok(RemoveCartItemResponse.Data);
        }

        [Authorize]
        [HttpDelete("ClearCart")]
        public  async Task<ActionResult<string>> ClearCartAsync()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }

            var ClearCartResponse = await userCart.ClearCartAsync(userId);
            if(ClearCartResponse.Success == false)
            {
                return BadRequest(ClearCartResponse.Message);
            }
            return Ok(ClearCartResponse.Data);
        }
    }
}

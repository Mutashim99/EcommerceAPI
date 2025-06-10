using EcommerceAPI.DTOs.OrderDTOs;
using EcommerceAPI.Services.UserManagement.UserOrderManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserOrderController : ControllerBase
    {
        private readonly IUserOrder userOrderService;

        public UserOrderController(IUserOrder userOrderService)
        {
            this.userOrderService = userOrderService;
        }

        [HttpGet("MyOrders")]
        public async Task<ActionResult<List<UserOrderResponseDTO>>> GetMyOrdersAsync()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }

            var response = await userOrderService.GetMyOrdersAsync(userId);
            if (response.Success == false)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpGet("OrderDetails/{orderId}")]
        public async Task<ActionResult<UserOrderDetailsResponseDTO>> GetOrderDetailsAsync(int orderId)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }

            var response = await userOrderService.GetOrderDetailsAsync(userId, orderId);
            if (response.Success == false)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpPost("CheckoutPreview")]
        public async Task<ActionResult<List<CheckoutPreviewResponseDTO>>> GetCheckoutPreviewAsync([FromBody] List<int> cartItemIds)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }

            var response = await userOrderService.GetCheckoutPreviewAsync(userId, cartItemIds);
            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpPost("place-order")]
        public async Task<ActionResult<int>> PlaceOrderAsync([FromBody] CreateOrderDTO request)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }

            var response = await userOrderService.PlaceOrderAsync(userId, request);
            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(new { OrderId = response.Data , Message = "Order Placed Succesfully"}); // Order ID
        }

        [HttpPost("cancel/{orderId}")]
        public async Task<ActionResult<string>> CancelOrderAsync(int orderId)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
            {
                return Unauthorized("User is not authorized");
            }

            var response = await userOrderService.CancelOrderAsync(userId, orderId);
            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }
    }
}

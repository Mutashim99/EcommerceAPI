using EcommerceAPI.Constants;
using EcommerceAPI.DTOs.AdminDTOs;
using EcommerceAPI.Services.AdminManagement.AdminOrder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminOrderController : ControllerBase 
    {
        private readonly IAdminOrder _adminOrderService;

        public AdminOrderController(IAdminOrder adminOrderService)
        {
            _adminOrderService = adminOrderService;
        }

        // GET: api/admin/orders
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _adminOrderService.GetAllOrdersAsync();
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        // GET: api/admin/orders/GetOrderByStatus?status=Delivered
        [HttpGet("GetOrderByStatus")]
        public async Task<IActionResult> GetOrdersByStatus([FromQuery] string status)
        {
            var result = await _adminOrderService.GetOrdersByStatusAsync(status);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        // GET: api/admin/orders/5
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var result = await _adminOrderService.GetOrderByIdAsync(orderId);
            return result.Success ? Ok(result.Data) : NotFound(result.Message);
        }

        // PUT: api/admin/orders/5/status
        [HttpPut("{orderId}/UpdateStatus")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusRequestDTO request)
        {
            var result = await _adminOrderService.UpdateOrderStatusAsync(orderId, request.NewStatus);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }
        // GET: api/admin/orders/AllStatuses
        [HttpGet("AllStatuses")]
        public IActionResult GetAllOrderStatuses()
        {
            var statuses = OrderStatuses.GetAllStatuses();
            return Ok(statuses);
        }

    }
}

using AutoMapper;
using EcommerceAPI.DTOs.AdminDTOs;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services.AdminManagement.AdminOrder
{
    public class AdminOrderService : IAdminOrder
    {
        private readonly AppDbContext db;
        private readonly IMapper mapper;

        public AdminOrderService(AppDbContext db , IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<ServiceResponse<List<AdminOrderResponseDTO>>> GetAllOrdersAsync()
        {
            var allOrders = await db.Orders.Include(x=> x.User).ToListAsync();
            var mappedResponse = mapper.Map<List<AdminOrderResponseDTO>>(allOrders);
            return new ServiceResponse<List<AdminOrderResponseDTO>>
            {
                Data = mappedResponse,
                Message = "All Orders Fetched",
                Success = true
            };
        }

        public async Task<ServiceResponse<AdminOrderDetailsResponseDTO>> GetOrderByIdAsync(int orderId)
        {
            var orderFromDb = await db.Orders
                .Include(x => x.User)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Product)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Variant)
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x=> x.Id == orderId);
            
            if(orderFromDb == null)
            {
                return new ServiceResponse<AdminOrderDetailsResponseDTO> 
                { 
                    Success = false,
                    Message = $"cant find any order with the Id : {orderId}",
                    Data = null
                };
            }
            var mappedtoDto = mapper.Map<AdminOrderDetailsResponseDTO>(orderFromDb);
            return new ServiceResponse<AdminOrderDetailsResponseDTO>
            {
                Data = mappedtoDto,
                Success = true,
                Message = "Order fetched succesfully"
            };

        }

        public async Task<ServiceResponse<List<AdminOrderResponseDTO>>> GetOrdersByStatusAsync(string status)
        {
            if(status == null)
            {
                return new ServiceResponse<List<AdminOrderResponseDTO>> 
                {
                    Data = null,
                    Message = "status is required",
                    Success = false
                };
            }

            var allOrders = await db.Orders.Include(x => x.User).Where(x=> x.OrderStatus == status).ToListAsync();
            var mappedResponse = mapper.Map<List<AdminOrderResponseDTO>>(allOrders);
            return new ServiceResponse<List<AdminOrderResponseDTO>>
            {
                Data = mappedResponse,
                Message = $"All Orders with status {status} Fetched",
                Success = true
            };
        }

        public async Task<ServiceResponse<string>> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var currentOrder = await db.Orders.FindAsync(orderId);
            if(currentOrder == null)
            {
                return new ServiceResponse<string>
                {
                    Data = null,
                    Message = $"cant find any order with the given Id {orderId}",
                    Success = false
                };
            }
            currentOrder.OrderStatus = newStatus;
            await db.SaveChangesAsync();
            return new ServiceResponse<string>
            {
                Data = $"Order Status Updated succesfully to {newStatus}",
                Success = true,
                Message = "Status updated"
            };
        }
    }
}

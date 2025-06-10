using AutoMapper;
using EcommerceAPI.DTOs.OrderDTOs;
using EcommerceAPI.Models;
using EcommerceAPI.Services.Email;
using EcommerceAPI.Services.UserManagement.UserAddressManagement;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services.UserManagement.UserOrderManagement
{
    public class UserOrderService : IUserOrder
    {
        private readonly AppDbContext db;
        private readonly IMapper mapper;
        private readonly IUserAddress userAddressService;
        private readonly IEmail emailService;
        public UserOrderService(AppDbContext db , IMapper mapper, IUserAddress userAddressService, IEmail emailService)
        {
            this.db = db;
            this.mapper = mapper;
            this.userAddressService = userAddressService;
            this.emailService = emailService;
        }

        public Task<ServiceResponse<List<UserOrderResponseDTO>>> GetMyOrdersAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<UserOrderDetailsResponseDTO>> GetOrderDetailsAsync(int userId, int orderId)
        {
            throw new NotImplementedException();
        }


        // this method is for the checkout page where we will pass all the selected cartItem ids from cart page into this method in an array to this method to show the selected items in checkout page
        public async Task<ServiceResponse<List<CheckoutPreviewResponseDTO>>> GetCheckoutPreviewAsync(int userId, List<int> cartItemIds)
        {
            var selectedCartItems = await db.CartItems
                .Include(x => x.Product)
                .Include(x=> x.Variant)
                .Where(x => x.UserId  == userId && cartItemIds.Contains(x.Id))
                .ToListAsync();

            if (!selectedCartItems.Any())
            {
                return new ServiceResponse<List<CheckoutPreviewResponseDTO>>
                {
                    Data = null,
                    Message = "No valid cart items found",
                    Success = false
                };
            }

            var mappedIntoPreviewResponse = mapper.Map<List<CheckoutPreviewResponseDTO>>(selectedCartItems);
            return new ServiceResponse<List<CheckoutPreviewResponseDTO>>
            {
                Data = mappedIntoPreviewResponse,
                Message = "All the cart Items with details fetched",
                Success = true
            };
        }

        //
        public async Task<ServiceResponse<int>> PlaceOrderAsync(int userId, CreateOrderDTO request)
        {
            if (request.SelectedCartItemIds == null || !request.SelectedCartItemIds.Any())
            {
                return new ServiceResponse<int> { Success = false, Message = "No cart items selected." };
            }

            var cartItems = await db.CartItems
                .Include(c => c.Product)
                .Include(c => c.Variant)
                .Where(c => c.UserId == userId && request.SelectedCartItemIds.Contains(c.Id))
                .ToListAsync();

            if (!cartItems.Any())
            {
                return new ServiceResponse<int> { Success = false, Message = "Invalid or empty cart items." };
            }

            // Determine address
            int finalAddressId;
            if (request.AddressId != 0)
            {
                var existingAddress = await db.Addresses.FirstOrDefaultAsync(a => a.Id == request.AddressId && a.UserId == userId);
                if (existingAddress == null)
                {
                    return new ServiceResponse<int> { Success = false, Message = "Selected address is not valid for this user." };
                }
                finalAddressId = existingAddress.Id;
            }
            else if (request.NewAddress != null)
            {
                var addressResponse = await userAddressService.AddAddressAsync(userId, request.NewAddress);
                if (!addressResponse.Success || addressResponse.Data == null)
                {
                    return new ServiceResponse<int> { Success = false, Message = "Unable to add new address. " + addressResponse.Message };
                }
                finalAddressId = addressResponse.Data.Id;
            }
            else
            {
                return new ServiceResponse<int> { Success = false, Message = "Address is required (either Add an address in your profile or add New Address here )." };
            }

            // 🧮 Calculate total
            decimal totalAmount = cartItems.Sum(ci => ci.Variant.Price * ci.Quantity);

            var order = new Order
            {
                UserId = userId,
                AddressId = finalAddressId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                OrderStatus = "Pending",
                PaymentMethod = request.PaymentMethod,
             
            };
            var orderItems = mapper.Map<List<OrderItem>>(cartItems);
            order.OrderItems = orderItems;


            await db.Orders.AddAsync(order);
            db.CartItems.RemoveRange(cartItems);
            await db.SaveChangesAsync();

            var currentUser = await db.Users.FirstOrDefaultAsync(x=> x.Id == userId);

            var orderEmailBody = $"<h1>Your Order has been placed succesfully</h1> <br> <p>Your Order Id is {order.Id} and its current status is {order.OrderStatus} </p>";
            var SendEmail = await emailService.SendEmailAsync(currentUser.Email, "Your Order has been Placed", orderEmailBody);

            return new ServiceResponse<int>
            {
                Success = true,
                Message = "Order placed successfully.",
                Data = order.Id
            };
        }

        public Task<ServiceResponse<bool>> CancelOrderAsync(int userId, int orderId)
        {
            throw new NotImplementedException();
        }



     

     

   
    }
}

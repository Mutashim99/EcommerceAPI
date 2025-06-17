using AutoMapper;
using EcommerceAPI.Constants;
using EcommerceAPI.DTOs.OrderDTOs;
using EcommerceAPI.Models;
using EcommerceAPI.Services.Email;
using EcommerceAPI.Services.UserManagement.UserAddressManagement;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;

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

        public async Task<ServiceResponse<List<UserOrderResponseDTO>>> GetMyOrdersAsync(int userId)
        {
            var allUserOrders = await db.Orders.Where(x=> x.UserId == userId).OrderByDescending(o => o.OrderDate).ToListAsync();

            if(!allUserOrders.Any())
    {
                return new ServiceResponse<List<UserOrderResponseDTO>>
                {
                    Success = false,
                    Message = "No orders found for this user.",
                    Data = null
                };
            }
            var MappedIntoUserOrderResponse = mapper.Map<List<UserOrderResponseDTO>>(allUserOrders);
            return new ServiceResponse<List<UserOrderResponseDTO>>
            {
                Success = true,
                Message = "Orders retrieved successfully.",
                Data = MappedIntoUserOrderResponse
            };
        }

        public async Task<ServiceResponse<UserOrderDetailsResponseDTO>> GetOrderDetailsAsync(int userId, int orderId)
        {
            var OrderDetail = await db.Orders
                .Include(x=> x.Address)
                .Include(x=> x.OrderItems)
                .ThenInclude(x=> x.Product)
                .Include(x => x.OrderItems)
                .ThenInclude(x=> x.Variant)
                .FirstOrDefaultAsync(x=> x.Id == orderId && x.UserId == userId);
            if (OrderDetail == null)
            {
                return new ServiceResponse<UserOrderDetailsResponseDTO>
                {
                    Success = false,
                    Message = "Order not found.",
                    Data = null
                };
            }
            var mappedOrderDetailIntoDTO = mapper.Map<UserOrderDetailsResponseDTO>(OrderDetail);
            return new ServiceResponse<UserOrderDetailsResponseDTO>
            {
                Success = true,
                Message = "Order details retrieved successfully.",
                Data = mappedOrderDetailIntoDTO
            };
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

        //user has to pay at the time of checkout if they select Online Transfer method 
        //then and only they can place order and then admin can confirm the payment and 
        //then they can set the status to pending or processing
        public async Task<ServiceResponse<int>> PlaceOrderAsync(int userId, CreateOrderDTO request)
        {
            if (request.SelectedCartItemIds == null || !request.SelectedCartItemIds.Any())
            {
                return new ServiceResponse<int> { Success = false, Message = "No cart items selected." };
            }

            if (request.PaymentMethod.ToLower() == "online transfer" && string.IsNullOrEmpty(request.OnlinePaymentProofImageURL))
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "Payment proof is required for online payments."
                };
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

            // Calculate total
            decimal totalAmount = cartItems.Sum(ci => ci.Variant.Price * ci.Quantity);
            
            var order = new Order
            {
                UserId = userId,
                AddressId = finalAddressId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                OrderStatus = request.PaymentMethod.ToLower() == "Online Transfer" ? "PaymentProcessing" : "Pending", // Set order status based on payment method
                                                                                                                      // If online payment, require screenshot and mark as PaymentProcessing
                                                                                                                      // If cash on delivery, set status as Pending

                PaymentMethod = request.PaymentMethod,
                OnlinePaymentProofImageURL = request.OnlinePaymentProofImageURL
             
            };
            
            var orderItems = mapper.Map<List<OrderItem>>(cartItems);
            order.OrderItems = orderItems;

            foreach (var cartItem in cartItems)
            {
                var variant = await db.ProductVariants.FirstOrDefaultAsync(v => v.Id == cartItem.ProductVariantId);
                if (variant == null)
                {
                    return new ServiceResponse<int>
                    {
                        Success = false,
                        Message = $"Product variant with ID {cartItem.ProductVariantId} not found."
                    };
                }

                if (variant.Stock < cartItem.Quantity)
                {
                    return new ServiceResponse<int>
                    {
                        Success = false,
                        Message = $"Insufficient stock for {cartItem.Product.Name} (Variant: {variant.Color}, {variant.Size})."
                    };
                }

                variant.Stock -= cartItem.Quantity; 
            }


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

        public async Task<ServiceResponse<string>> CancelOrderAsync(int userId, int orderId)
        {
            var CurrentOrder = await db.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.UserId == userId);
            if (CurrentOrder == null)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Order Does not exist",
                    Data = "The Order that you are trying to delete does not exist"
                };
            }
            if (string.Equals(CurrentOrder.OrderStatus, OrderStatuses.Processing, StringComparison.OrdinalIgnoreCase))
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Order cannot be cancelled while processing.",
                    Data = "The order status is currently processing. Contact support for further help."
                };
            }

            if (string.Equals(CurrentOrder.OrderStatus, OrderStatuses.Shipped, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(CurrentOrder.OrderStatus, OrderStatuses.Delivered, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(CurrentOrder.OrderStatus, OrderStatuses.Cancelled, StringComparison.OrdinalIgnoreCase))
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "This order cannot be cancelled.",
                    Data = $"Order is already {CurrentOrder.OrderStatus}."
                };
            }


            CurrentOrder.OrderStatus = "Cancelled";

            var currentUser = await db.Users.FirstOrDefaultAsync(x => x.Id == userId);

            await  db.SaveChangesAsync();

            await emailService.SendEmailAsync(currentUser.Email, "Order Cancelled", $"Your order #{CurrentOrder.Id} has been cancelled.");

            return new ServiceResponse<string>
            {
                Success = true,
                Message = "Order Cancelled Succesfully",
                Data = $"Your Order {CurrentOrder.Id} has been {CurrentOrder.OrderStatus} succesfully"
            };
        }



     

     

   
    }
}

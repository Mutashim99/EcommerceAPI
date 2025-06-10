using AutoMapper;
using EcommerceAPI.DTOs.AuthDTOs;
using EcommerceAPI.DTOs.CategoryDTOs;
using EcommerceAPI.DTOs.ProductDTOs;
using EcommerceAPI.DTOs.ProductVariantDTOs;
using EcommerceAPI.DTOs.ReviewDTOs;
using EcommerceAPI.DTOs.UserAddressDTOs;
using EcommerceAPI.DTOs.UserCartDTOs;
using EcommerceAPI.DTOs.UserFavoriteDTOs;
using EcommerceAPI.DTOs.UserProfileDTOs;
using EcommerceAPI.Models;

namespace EcommerceAPI.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRegistrationDTO, User>()
    .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<User, UserRegistrationDTO>(); 

            CreateMap<UserLoginDTO, UserLoginDTO>();

            CreateMap<User,UserProfileDTO>();

            CreateMap<CreateAddressDto, Address>();
            CreateMap<Address,AddressResponseDto>();

            CreateMap<Product, ProductResponseDTO>();
            CreateMap<Category, CategoryResponseDTO>();
            CreateMap<Review, ReviewResponseDTO>();
            CreateMap<ProductVariant, ProductVariantResponseDTO>();
            CreateMap<CartItem,CartItemResponseDTO>();

            CreateMap<CreateCartItemDTO, CartItem>();

            CreateMap<FavoriteItem, FavoriteResponseDTO>();
            

            CreateMap<Review, UserReviewResponseDTO>();
            CreateMap<CreateReviewDTO, Review>();
            CreateMap<OrderItem, PendingReviewProductDTO>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.Product.ImageURL))
            .ForMember(dest => dest.OrderTotalAmount, opt => opt.MapFrom(src => src.Order.TotalAmount))
            .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Order.OrderStatus));

        }
    }
}

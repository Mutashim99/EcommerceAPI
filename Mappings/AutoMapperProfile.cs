using AutoMapper;
using EcommerceAPI.DTOs.AuthDTOs;
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
        }
    }
}

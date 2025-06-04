using AutoMapper;
using EcommerceAPI.DTOs;
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
        }
    }
}

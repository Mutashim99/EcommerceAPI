using AutoMapper;
using EcommerceAPI.DTOs;
using EcommerceAPI.Models;

namespace EcommerceAPI.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRegistrationDTO, User>();
            CreateMap<User, UserRegistrationDTO>(); 
        }
    }
}

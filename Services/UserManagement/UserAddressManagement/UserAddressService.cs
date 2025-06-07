using AutoMapper;
using EcommerceAPI.DTOs.UserAddressDTOs;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services.UserManagement.UserAddressManagement
{
    public class UserAddressService : IUserAddress
    {
        private readonly AppDbContext db;
        private readonly IMapper mapper;
        public UserAddressService(AppDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
        public async Task<ServiceResponse<List<AddressResponseDto>>> GetUserAddressesAsync(int UserId)
        {
            var AllAddresses = await db.Addresses.Where(x => x.UserId == UserId).ToListAsync();
            
            var ResponseAddress = mapper.Map<List<AddressResponseDto>>(AllAddresses);

            return new ServiceResponse<List<AddressResponseDto>>
            {
                Data = ResponseAddress,
                Message = "User addresses fetched",
                Success = true
            };
        }
        public async  Task<ServiceResponse<AddressResponseDto>> AddAddressAsync(int UserId , CreateAddressDto createAddressDto)
        {
            if (createAddressDto == null)
            {
                return new ServiceResponse<AddressResponseDto>
                {
                    Data = null,
                    Message = "Incorrect or Null Data",
                    Success = false
                };
            }
            var address = mapper.Map<Address>(createAddressDto);
            address.UserId = UserId;

            var existingAddress = await db.Addresses.FirstOrDefaultAsync(a =>
                a.UserId == UserId &&
                a.Label.ToLower().Trim() == createAddressDto.Label.ToLower().Trim() &&
                a.Street.ToLower().Trim() == createAddressDto.Street.ToLower().Trim() &&
                a.City.ToLower().Trim() == createAddressDto.City.ToLower().Trim() &&
                a.State.ToLower().Trim() == createAddressDto.State.ToLower().Trim() &&
                a.PostalCode.ToLower().Trim() == createAddressDto.PostalCode.ToLower().Trim() &&
                a.Country.ToLower().Trim() == createAddressDto.Country.ToLower().Trim());
            if (existingAddress != null)
            {
                return new ServiceResponse<AddressResponseDto>
                {
                    Data = mapper.Map<AddressResponseDto>(existingAddress),
                    Message = "Address with the same details already exists.",
                    Success = false
                };
            }
            if (createAddressDto.IsDefault)
            {
                var existingDefault = await db.Addresses.FirstOrDefaultAsync(a =>
                    a.UserId == UserId && a.IsDefault);

                if (existingDefault != null)
                {
                    return new ServiceResponse<AddressResponseDto>
                    {
                        Data = null,
                        Message = "Only one default address is allowed.",
                        Success = false
                    };
                }
            }
            await db.Addresses.AddAsync(address);
            await db.SaveChangesAsync();
            var addressResponseDto = mapper.Map<AddressResponseDto>(address);

            return new ServiceResponse<AddressResponseDto>
            {
                Data = addressResponseDto,
                Message = "Address added succesfully",
                Success = true
            };
            
        }
        public async Task<ServiceResponse<AddressResponseDto>> UpdateAddressAsync(int userId, int addressId, CreateAddressDto updateAddressDto)
        {
            if (updateAddressDto == null || addressId <= 0)
            {
                return new ServiceResponse<AddressResponseDto>
                {
                    Data = null,
                    Message = "Invalid address ID or update data",
                    Success = false
                };
            }

            var existingAddress = await db.Addresses.FirstOrDefaultAsync(x => x.Id == addressId && x.UserId == userId);
            if (existingAddress == null)
            {
                return new ServiceResponse<AddressResponseDto>
                {
                    Data = null,
                    Message = "Address not found",
                    Success = false
                };
            }

            // Map updated fields from DTO to existing address entity
            mapper.Map(updateAddressDto, existingAddress);

            db.Addresses.Update(existingAddress);
            await db.SaveChangesAsync();

            var responseDto = mapper.Map<AddressResponseDto>(existingAddress);

            return new ServiceResponse<AddressResponseDto>
            {
                Data = responseDto,
                Message = "Address updated successfully",
                Success = true
            };
        }


        public async Task<ServiceResponse<string>> DeleteAddressAsync(int userId, int addressId)
        {
            if(addressId <= 0)
            {
                return new ServiceResponse<string>
                {
                    Data = null,
                    Message = "Incorrect or Null Address ID",
                    Success = false
                };
            
            }
            var CurrentAddress = await db.Addresses.FirstOrDefaultAsync(x => x.Id == addressId && x.UserId == userId);
            if(CurrentAddress == null)
            {
                return new ServiceResponse<string>
                {
                    Data = null,
                    Message = "Can not find any address with your ID and Address ID",
                    Success = false
                };
            }
            db.Addresses.Remove(CurrentAddress);
            await db.SaveChangesAsync();

            return new ServiceResponse<string>
            {
                Data = "Address deleted successfully",
                Message = "Deleted",
                Success = true
            };
        }
    }
}

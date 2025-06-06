using EcommerceAPI.DTOs.UserAddressDTOs;

namespace EcommerceAPI.Services.UserManagement.UserAddressManagement
{
    public class UserAddressService : IUserAddress
    {
        public Task<ServiceResponse<List<AddressResponseDto>>> GetUserAddressesAsync(int UserId)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponse<AddressResponseDto>> AddAddressAsync(CreateAddressDto createAddressDto)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponse<AddressResponseDto>> UpdateAddressAsync(int UserId, int addressId, CreateAddressDto updateAddressDto)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponse<string>> DeleteAddressAsync(int addressId)
        {
            throw new NotImplementedException();
        }
    }
}

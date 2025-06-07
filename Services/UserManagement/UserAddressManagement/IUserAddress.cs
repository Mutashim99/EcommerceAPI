using EcommerceAPI.DTOs.UserAddressDTOs;

namespace EcommerceAPI.Services.UserManagement.UserAddressManagement
{
    public interface IUserAddress
    {
        public Task<ServiceResponse<List<AddressResponseDto>>> GetUserAddressesAsync(int UserId);
        public Task<ServiceResponse<AddressResponseDto>> AddAddressAsync(int UserId , CreateAddressDto createAddressDto);
        public Task<ServiceResponse<AddressResponseDto>> UpdateAddressAsync(int UserId, int addressId, CreateAddressDto updateAddressDto);
        public Task<ServiceResponse<string>> DeleteAddressAsync(int userId ,int addressId);
    }
}

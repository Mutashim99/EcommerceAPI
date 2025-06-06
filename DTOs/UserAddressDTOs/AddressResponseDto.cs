namespace EcommerceAPI.DTOs.UserAddressDTOs
{
    public class AddressResponseDto
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Label { get; set; }
        public bool IsDefault { get; set; }
    }

}

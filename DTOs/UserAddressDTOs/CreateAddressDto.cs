using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs.UserAddressDTOs
{
    public class CreateAddressDto
    {
        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Country { get; set; }

        public string Label { get; set; } = "Home";

        public bool IsDefault { get; set; } = false;
    }

}

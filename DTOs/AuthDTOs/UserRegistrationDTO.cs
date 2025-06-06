using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs.AuthDTOs
{
    public class UserRegistrationDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }

        public string Role { get; set; } = "Customer";
    }
}

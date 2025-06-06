using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs.UserProfileDTOs
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

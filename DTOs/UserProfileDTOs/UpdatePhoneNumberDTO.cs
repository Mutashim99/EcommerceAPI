using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs.UserProfileDTOs
{
    public class UpdatePhoneNumberDTO
    {
        [Required]
        [Phone]
        public string NewPhoneNumber { get; set; }
    }
}

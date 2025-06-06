using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs.UserProfileDTOs
{
    public class UpdateUsernameDTO
    {
        [Required]
        public string NewName { get; set; }
    }
}

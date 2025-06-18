using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs.AdminDTOs
{
    public class UpdateOrderStatusRequestDTO
    {
        [Required]
        public string NewStatus { get; set; }
    }

}

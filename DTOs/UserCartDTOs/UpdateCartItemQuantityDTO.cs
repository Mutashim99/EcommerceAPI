using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs.UserCartDTOs
{
    public class UpdateCartItemQuantityDTO
    {
        [Required]
        public int NewQuantity { get; set; }
        [Required]
        public int CartItemId { get; set; }
    }
}

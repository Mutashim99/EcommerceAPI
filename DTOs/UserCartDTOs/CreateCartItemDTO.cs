using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs.UserCartDTOs
{
    public class CreateCartItemDTO
    {
        [Required]
        public int ProductId { get; set; }
        public int ProductVariantId { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

    }

}

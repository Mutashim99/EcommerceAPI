using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs.ReviewDTOs
{
    public class CreateReviewDTO
    {
        [Range(1, 5)]
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public int ProductId { get; set; }
    }
}

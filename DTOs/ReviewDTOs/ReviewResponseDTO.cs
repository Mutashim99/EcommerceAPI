using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs.ReviewDTOs
{
    public class ReviewResponseDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

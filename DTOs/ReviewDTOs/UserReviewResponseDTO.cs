namespace EcommerceAPI.DTOs.ReviewDTOs
{
    public class UserReviewResponseDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public ReviewProductDTO Product { get; set; }
    }
}

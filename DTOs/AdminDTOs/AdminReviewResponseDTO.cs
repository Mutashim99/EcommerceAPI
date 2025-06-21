namespace EcommerceAPI.DTOs.AdminDTOs
{
    public class AdminReviewResponseDTO
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserName { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
    }
}

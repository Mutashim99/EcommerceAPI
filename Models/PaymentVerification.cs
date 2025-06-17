namespace EcommerceAPI.Models
{
    public class PaymentVerification
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public string PaymentProofImageUrl { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public string? AdminNote { get; set; }
    }
}

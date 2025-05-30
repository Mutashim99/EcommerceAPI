using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Country { get; set; }

        public string Label { get; set; } = "Home";

        public int UserId { get; set; } // 1 user can have many addresses so that he can select at checkout page which oneb he wants to choose for the delivery of this order
        public User User { get; set; } // Jis user ka address he wo
    }
}

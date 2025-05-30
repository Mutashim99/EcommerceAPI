namespace EcommerceAPI.Models
{
    public class FavoriteItem
    {
        public int Id { get; set; }

        public int UserId { get; set; } // jis user ne add kya product ko as favorite uski id(FK)
        public User User { get; set; } // user 


        public int ProductId { get; set; } // jo product as favourite add hua he uski id(FK)
        public Product FavoriteProduct { get; set; } // jo product hua add favorite
    } 
}

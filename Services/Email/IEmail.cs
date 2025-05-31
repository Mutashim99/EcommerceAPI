namespace EcommerceAPI.Services.Email
{
    public interface IEmail
    {
        public Task<bool> SendEmailAsync(string to,string subject,string body);
    }
}

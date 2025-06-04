
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace EcommerceAPI.Services.Email
{
    public class EmailService : IEmail
    {
        private readonly IConfiguration config;

        public EmailService(IConfiguration config)
        {
            this.config = config;
        }
        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var SmtpServer = config["EmailSettings:SmtpServer"];
                var Port = int.Parse(config["EmailSettings:Port"]);
                var SenderEmail = config["EmailSettings:SenderEmail"];
                var SenderName = config["EmailSettings:SenderName"];
                var password = config["EmailSettings:Password"];

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(SenderName, SenderEmail));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart("html") { Text = body };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(SmtpServer, Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(SenderEmail, password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Failed to send email: {ex.Message}");
                return false;
            }
        }

    }
}

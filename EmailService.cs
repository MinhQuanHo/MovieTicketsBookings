using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace MovieTicketsBooking
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");
                string server = smtpSettings["Server"];
                string portString = smtpSettings["Port"];
                string username = smtpSettings["Username"];
                string password = smtpSettings["Password"];
                string senderEmail = smtpSettings["SenderEmail"];
                string senderName = smtpSettings["SenderName"];

                if (string.IsNullOrWhiteSpace(server) || string.IsNullOrWhiteSpace(portString) ||
                    string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                    string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(email))
                {
                    throw new ArgumentException("SMTP configuration or recipient email is missing or invalid.");
                }

                if (!int.TryParse(portString, out int port))
                {
                    throw new FormatException("Invalid SMTP port format.");
                }

                using var smtpClient = new SmtpClient(server)
                {
                    Port = port,
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true,
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
    }
}

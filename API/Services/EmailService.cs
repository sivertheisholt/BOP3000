using API.Interfaces.IServices;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace API.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _client;
        
        private readonly string _email;
        
        public EmailService()
        {
            var client = new SmtpClient();
            var email = Environment.GetEnvironmentVariable("EMAIL_USERNAME");
            var password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");

            client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            client.Authenticate(email, password);

            _client = client;
            _email = email;
        }

        public bool SendForgottenPasswordMail(string token, string email)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Playfu", _email));
            message.To.Add(new MailboxAddress("Member", email));
            message.Subject = "Password reset request";
            message.Body = new TextPart("plain")
            {
                Text = $"Reset link: {token}"
            };

            _client.Send(message);

            return true;
        }
    }
}
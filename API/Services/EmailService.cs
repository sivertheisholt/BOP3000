using API.DTOs.Support;
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

        public EmailService(IConfiguration config)
        {
            var client = new SmtpClient();

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var username = "";
            var password = "";
            if (env == "Development")
            {

                username = config.GetSection("PLAYFU_EMAIL_ACCOUNT")["PLAYFU_EMAIL_ACCOUNT_USERNAME"];
                password = config.GetSection("PLAYFU_EMAIL_ACCOUNT")["PLAYFU_EMAIL_ACCOUNT_PASSWORD"];
            }
            else
            {
                username = Environment.GetEnvironmentVariable("PLAYFU_EMAIL_ACCOUNT_USERNAME");
                password = Environment.GetEnvironmentVariable("PLAYFU_EMAIL_ACCOUNT_PASSWORD");
            }

            client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            client.Authenticate(username, password);

            _client = client;
            _email = username;
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

        public Task SendNewTicketMail(NewTicketDto ticketDto)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Playfu", _email));
            message.To.Add(new MailboxAddress("support", "support@playfu.freshdesk.com"));
            message.Subject = ticketDto.Subject;
            message.Body = new TextPart("plain")
            {
                Text = $"Email: {ticketDto.Email}, Name: {ticketDto.Name}, Description: {ticketDto.Description}"
            };
            _client.Send(message);
            return Task.CompletedTask;
        }
    }
}
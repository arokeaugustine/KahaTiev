using KahaTiev.DTOs;
using KahaTiev.Services.Interfaces;
using RestSharp;
using System.Net;
using System.Net.Mail;

namespace KahaTiev.Services
{

    public class MailService : IMailService
    {
        private readonly String _apiKey;

        private readonly String _emailBaseUrl;
        public MailService(IConfiguration config)
        {
            _apiKey = config["Mail:KahaMail"];
            _emailBaseUrl = config["Mail:BaseUrl"];
        }

        public async Task<bool> SendMail(MailDTO mail)
        {
            if (mail == null)
            {
                return false;

            }
            string fromAddress = "augustine@bineops.com";
            string smtpHost = "smtp.mailbit.io";
            int smtpPort = 500;
            string smtpUsername = "arokeaugustine@gmail.com";
            string smtpPassword = "IamAdeiza1.";

            SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = false
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = mail.Subject,
                Body = mail.Body,

            };
            mailMessage.To.Add(mail.ToAddress);

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successful");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }
    }
}

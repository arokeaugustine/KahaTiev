using KahaTiev.Data.DTOs;

namespace KahaTiev.Services.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendMail(MailDTO mail);
    }
}

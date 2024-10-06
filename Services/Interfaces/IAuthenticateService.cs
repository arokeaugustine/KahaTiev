using KahaTiev.DTOs;

namespace KahaTiev.Services.Interfaces
{
    public interface IAuthenticateService
    {
        Task<Response> Register(UserRegistrationDTO userRegistration);
        Task<Response> Login(LoginDTO login);
       
    }
}

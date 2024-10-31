using KahaTiev.Data.DTOs;

namespace KahaTiev.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Response> Register(UserRegistrationDTO userRegistration);
        Task<LoginResponseDTO> Login(LoginDTO login);
        Task<Response> ChangePassword(ChangePasswordDTO changePassword);


    }
}

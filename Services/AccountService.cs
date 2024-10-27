using KahaTiev.DTOs;
using KahaTiev.Models;
using KahaTiev.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KahaTiev.Contact.Services
{
    public class AccountService : IAccountService
    {
        private readonly KahaTievContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailservice;
        public AccountService(KahaTievContext context, IConfiguration configuration, IMailService mailservice)
        {
            _context = context;
            _configuration = configuration;
            _mailservice = mailservice;
        }

        public async Task<Response> Register(UserRegistrationDTO userRegistration)
        {

            if (userRegistration == null)
            {
                return new Response
                {
                    Status = false,
                    Message = "Invalid Data"
                };
            }

            if (!string.Equals(userRegistration.Password, userRegistration.ConfirmPassword))
            {
                return new Response
                {
                    Status = false,
                    Message = "Provided passwords must correlate!"
                };
            }
            var checker = _context.Users.Any(x => x.EmailAddress == userRegistration.EmailAddress);
            if (checker)
            {
                return new Response
                {
                    Status = false,
                    Message = "Duplicate user not allowed!"
                };

            }

            var user = new User
            {
                FirstName = userRegistration.FirstName,
                LastName = userRegistration.LastName,
                EmailAddress = userRegistration.EmailAddress,
                Password = BCrypt.Net.BCrypt.HashPassword(userRegistration.Password, workFactor: 12),
                RoleId = 2
            };

            _context.Users.Add(user);
            var save = await _context.SaveChangesAsync();
            if (save < 0)
            {
                return new Response
                {
                    Status = false,
                    Message = "An error occured while trying to create this account"
                };
            }

            var sendMail = await SendWelcomeMail(user);

            return new Response
            {
                Status = true,
                Message = "Account creation successful"
            };
        }

        public async Task<LoginResponseDTO> Login(LoginDTO login)
        {
            var user = await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.EmailAddress == login.Username && x.IsActive && !x.IsDeleted);

            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {
                return new LoginResponseDTO
                {
                    Status = false,
                    Message = "Invalid username or password!"
                };

            }

            return new LoginResponseDTO
            {
                Status = true,
                Message = "authenticated successful",
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                RoleId = user.RoleId,
                RoleName = user.Role.Name
            };

        }

        public async Task<Response> ChangePassword(ChangePasswordDTO changePassword)
        {
            if (changePassword == null)
            {
                return new Response
                {
                    Status = false,
                    Message = "old password and new password must be provided!"
                };
            }

            if (string.IsNullOrEmpty(changePassword.ConfirmNewPassword) || string.IsNullOrEmpty(changePassword.NewPassword) || string.IsNullOrEmpty(changePassword.OldPassword))
            {
                return new Response
                {
                    Status = false,
                    Message = "old password and new password must be provided!"
                };
            }

            if (!string.Equals(changePassword.ConfirmNewPassword, changePassword.NewPassword))
            {
                return new Response
                {
                    Status = false,
                    Message = "New password and confirm password must be the same!"
                };
            }

            var user = await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.EmailAddress == changePassword.Email && x.IsActive && !x.IsDeleted);

            if (user == null)
            {
                return new Response
                {
                    Status = false,
                    Message = "Currently unable to retrieve user details!"
                };

            }

            var confirmPassword = BCrypt.Net.BCrypt.Verify(changePassword.OldPassword, user.Password);
            if (!confirmPassword)
            {
                return new Response
                {
                    Status = false,
                    Message = "Old password is invalid!"
                };
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword, workFactor: 12);
            var save = _context.SaveChanges();
            if (save > 0)
            {
                return new Response
                {
                    Status = true,
                    Message = "Password changes successfully"
                };
            }
            return new Response
            {
                Status = false,
                Message = "An error occurred while saving"
            };

        }



        private async Task<bool> SendWelcomeMail(User user)
        {
            var mailbody = $"Dear {user.FirstName} <br><br>" +
                "It is our pleasure to have you onboard our platform as we go through this agricultural revolution together" +
                "Please receive our warmest regards" +
                "<br>" +
                "Best regards,";

            var mailrequest = new MailDTO
            {
                Subject = "Welcome to KahaTiev",
                Body = mailbody,
                ToAddress = user.EmailAddress
            };

            return await _mailservice.SendMail(mailrequest);

        }
    }
}

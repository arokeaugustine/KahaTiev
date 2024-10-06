using KahaTiev.DTOs;
using KahaTiev.Models;
using KahaTiev.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KahaTiev.Contact.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly KahaTievContext _context;
        private readonly IConfiguration _configuration;
        public AuthenticateService(KahaTievContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<Response> Register(UserRegistrationDTO userRegistration)
        {

            if (userRegistration == null)
            {
                return new Response
                {
                    status = false,
                    message = "Invalid data"
                };
            }

            if (!string.Equals(userRegistration.Password, userRegistration.ConfirmPassword))
            {
                return new Response
                {
                    status = false,
                    message = "Provided passwords must correlate!"
                };
            }
            var checker = _context.Users.Any(x => x.EmailAddress == userRegistration.EmailAddress);
            if (checker)
            {
                return new Response
                {
                    status = false,
                    message = "Duplicate user not allowed!"
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
            if (save > 0)
            {
                return new Response
                {
                    status = true,
                    message = "Account creation successful"
                };
            }

            return new Response
            {
                status = false,
                message = "An error occured while trying to create this account"
            };
        }

        public async Task<Response> Login(LoginDTO login)
        {
            var user = await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.EmailAddress == login.username && x.IsActive && !x.IsDeleted);

            if (user == null || !BCrypt.Net.BCrypt.Verify(login.password, user.Password))
            {
                return new Response
                {
                    status = false,
                    message = "Invalid username or password!"
                };

            }

            var token = GenerateJwtToken(user);
            return new Response
            {
                status = true,
                message = token
            };

        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.EmailAddress),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(75),
                signingCredentials: cred
                );



            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

using KahaTiev.DTOs;
using KahaTiev.Models;
using KahaTiev.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KahaTiev.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly IAuthenticateService _authenticateService;

        public AuthenticateController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        public IActionResult Login() => View();


        public IActionResult Register() => View();
  
        public IActionResult AccessDenied()
        {
            return View(); 
        }

        public async Task<IActionResult> UserRegistration(UserRegistrationDTO userRegistration)
        {
            var response = await _authenticateService.Register(userRegistration).ConfigureAwait(false);
            return RedirectToAction("Login");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserLogin(LoginDTO login)
        {
            var response = await _authenticateService.Login(login);

            if (!response.Status)
            {
                TempData["ErrorMessage"] = "Invalid login credentials.";
                return RedirectToAction("Login");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, response.EmailAddress),
                new Claim(ClaimTypes.Role, response.RoleName),
                new Claim(ClaimTypes.Name, $"{response.FirstName} {response.LastName}"),

            };

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.Now.AddMinutes(30), 
                AllowRefresh = true
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            TempData["Login"] = "Login Successful";
            return RedirectToAction("Index", "Home");
        }


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

    }
}

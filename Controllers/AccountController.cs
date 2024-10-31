using KahaTiev.Data.DTOs;
using KahaTiev.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace KahaTiev.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Login() => View();


        public IActionResult Register() => View();
  
        public IActionResult AccessDenied()
        {
            return View(); 
        }

        public async Task<IActionResult> UserRegistration(UserRegistrationDTO userRegistration)
        {
            var response = await _accountService.Register(userRegistration).ConfigureAwait(false);
            return RedirectToAction("Login");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserLogin(LoginDTO login)
        {
            var response = await _accountService.Login(login);

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
             //   new Claim(ClaimTypes.use)

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

        [Authorize]
        public async ValueTask<IActionResult> ResetPassword()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePassword)
        {
            TempData["changePassword"] = null;
            if (!User.Identity!.IsAuthenticated)
            {
                TempData["changePassword"] = "Unable to retrieve user details";
                return View(changePassword);
            }
            changePassword.Email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var response  = await _accountService.ChangePassword(changePassword);
            TempData["changePassword"] = response;
            if (response.Status)
            {
                return RedirectToAction("Index", "Home");
            }
          
            return RedirectToAction(nameof(ResetPassword));
        }

    }
}

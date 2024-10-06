using KahaTiev.DTOs;
using KahaTiev.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KahaTiev.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly IAuthenticateService _authenticateService;

        public AuthenticateController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        public IActionResult Login() =>  View();

        public IActionResult Register() => View();

        public async Task<IActionResult> UserRegistration(UserRegistrationDTO userRegistration)
        {
            var response = await _authenticateService.Register(userRegistration).ConfigureAwait(false);
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> UserLogin(LoginDTO login)
        {
            var response = await _authenticateService.Login(login).ConfigureAwait(false);
            return RedirectToAction("Index", "Home");
        }
    }
}

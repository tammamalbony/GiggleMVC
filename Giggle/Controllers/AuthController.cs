using Giggle.Models.DomainModels;
using Giggle.Services;
using Microsoft.AspNetCore.Mvc;

namespace Giggle.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        private readonly TokenService _tokenService;

        public AuthController(AuthService authService, TokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterUserAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _authService.ValidateUserAsync(model);
                if (user != null)
                {
                    var token = _tokenService.GenerateToken(user.Username, user.Role, user.IsVerified);
                    HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions { HttpOnly = true, Secure = true });
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid username or password");
            }
            return View(model);
        }
    }
}

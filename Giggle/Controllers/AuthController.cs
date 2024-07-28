using Giggle.Models.DomainModels;
using Giggle.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet("login")]
        public IActionResult Login()
        {
            //_tokenService.generatingRSAkeys();
            //_tokenService.TestToken();
            return View();
        }

        [HttpPost("login")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {

           
            if (ModelState.IsValid)
            {
                var user = await _authService.ValidateUserAsync(model);
                if (user != null)
                {
                    var token = _tokenService.GenerateToken(user.Username, user.Role, user.IsVerified);
                    var isValidToken = _tokenService.ValidateToken(token);
                    _tokenService.SetJwtCookie(response: HttpContext.Response, token);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid username or password");
            }
            return View(model);
        }


        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete("jwt");
            return RedirectToAction("Login");

        }



    }
}

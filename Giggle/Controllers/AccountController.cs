using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Giggle.Models.DomainModels;
using Giggle.Services;
using System.Threading.Tasks;

namespace Giggle.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserService _userService;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);
                if (result.Success)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }
            return View(model);
        }

		[HttpGet("Register")]
		public IActionResult Register()
		{
			return View(new RegisterModel()); 
		}

		[HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _userService.RegisterUserAsync(model);
            if (result.Success)
            {
                var user = new IdentityUser { UserName = model.Username, Email = model.Email };
                var identityResult = await _userManager.CreateAsync(user, model.Password);
                if (identityResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }

            return View(model);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _userService.LogoutUserAsync();
            if (result.Success)
            {
                return RedirectToAction("Index", "Home");
            }
            return BadRequest("Error logging out");
        }

        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet("CheckEmail")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var isUnique = await _userService.IsEmailUniqueAsync(email);
            return Json(new { isUnique });
        }

        [HttpGet("CheckUsername")]
        public async Task<IActionResult> CheckUsername(string username)
        {
            var isUnique = await _userService.IsUsernameUniqueAsync(username);
            return Json(new { isUnique });
        }
    }
}

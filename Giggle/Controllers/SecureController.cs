using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Giggle.Controllers
{
    [Authorize]
    public class SecureController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                foreach (var claim in claimsIdentity.Claims)
                {
                    Console.WriteLine($"{claim.Type}: {claim.Value}");
                }
            }
            return View("Index"); // Assuming a simple view that confirms the action
        }

        [HttpGet]
        [Authorize(Policy = "VerifiedUser")]
        public IActionResult VerifiedUser()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                foreach (var claim in claimsIdentity.Claims)
                {
                    Console.WriteLine($"{claim.Type}: {claim.Value}");
                }
            }
            return View("Index"); // Assuming the same simple view
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Test()
        {

            if (User.Identity.IsAuthenticated)
            {
                // The user is authenticated
                if (User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Administrator"))
                {
                    // The user has the required claim to be an administrator
                }
            }
            return View("Index"); // Assuming the same simple view
        }
    }
}

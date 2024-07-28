using Microsoft.AspNetCore.Authorization;

namespace Giggle.Identity.Claims
{
    public class HasClaimHandler : AuthorizationHandler<HasClaimRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasClaimRequirement requirement)
        {
            var hasClaim = context.User.Claims.Any(c => c.Type == requirement.ClaimType && c.Value == requirement.ClaimValue);
            if (hasClaim)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

}

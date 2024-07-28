using Microsoft.AspNetCore.Authorization;

namespace Giggle.Identity.Claims
{
    public class HasClaimRequirement : IAuthorizationRequirement
    {
        public string ClaimType { get; }
        public string ClaimValue { get; }

        public HasClaimRequirement(string claimType, string claimValue)
        {
            ClaimType = claimType;
            ClaimValue = claimValue;
        }
    }

}

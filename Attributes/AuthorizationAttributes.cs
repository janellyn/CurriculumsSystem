using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace TaskAuthenticationAuthorization.Attributes
{
    public class BuyerClaimsRequirement : IAuthorizationRequirement
    {
        public string[] RequiredClaims { get; }

        public BuyerClaimsRequirement(params string[] requiredClaims)
        {
            RequiredClaims = requiredClaims;
        }
    }

    public class BuyerClaimsHandler : AuthorizationHandler<BuyerClaimsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BuyerClaimsRequirement requirement)
        {
            if (context.User.Identity != null && context.User.Claims != null)
            {
                foreach (var claimType in requirement.RequiredClaims)
                {
                    if (context.User.Claims.Any(c => c.Type == claimType))
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}

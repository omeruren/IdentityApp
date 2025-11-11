using AspNetCoreIdentityApp.Repository.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.ClaimProviders
{
    public class UserClaimProvider : IClaimsTransformation
    {
        private readonly UserManager<AppUser> _userManager;

        public UserClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal != null && principal.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = principal.Identity as ClaimsIdentity;

                AppUser user = await _userManager.FindByNameAsync(identity.Name);

                if (user != null)
                {
                    if (user.BirthDate != null)
                    {
                        var today = DateTime.Today;
                        var age = today.Year - user.BirthDate?.Year;

                        if (age > 15)
                        {
                            Claim ViolenceClaim = new Claim("violence", true.ToString(), ClaimValueTypes.String, "Internal");

                            identity.AddClaim(ViolenceClaim);
                        }
                    }

                    if (user.City != null)
                    {
                        if (!principal.HasClaim(c => c.Type == "city"))
                        {
                            Claim CityClaim = new Claim("city", user.City, ClaimValueTypes.String, "Internal");

                            identity.AddClaim(CityClaim);
                        }
                    }
                }
            }

            return principal;
        }
    }
}

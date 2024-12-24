using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
namespace TrainingX_2.Areas.Identity.Data
{
    public class CustomSignInManager : SignInManager<User>
    {
        public CustomSignInManager(UserManager<User> userManager,
                               IHttpContextAccessor contextAccessor,
                               IUserClaimsPrincipalFactory<User> claimsFactory,
                               IOptions<IdentityOptions> optionsAccessor,
                               ILogger<SignInManager<User>> logger,
                               IAuthenticationSchemeProvider schemes)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, null)
        {
        }

        public override async Task<SignInResult> CheckPasswordSignInAsync(User user, string password, bool lockoutOnFailure)
        {
            if (user.Status_user == Status.ไม่ใช้งาน)
            {
                return SignInResult.NotAllowed; // User is inactive, not allowed to sign in
            }

            return await base.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
        }
    }
}

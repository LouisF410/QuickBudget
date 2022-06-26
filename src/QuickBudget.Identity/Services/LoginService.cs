using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using QuickBudget.Domain.Entities;
using System.Security.Claims;

namespace QuickBudget.Identity.Services
{
    public class LoginService : ILoginService<ApplicationUser>
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public LoginService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ApplicationUser> FindByUsername(string user)
        {
            return await _userManager.FindByEmailAsync(user);
        }

        public async Task<bool> ValidateCredentials(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public Task SignIn(ApplicationUser user)
        {
            return _signInManager.SignInAsync(user, true);
        }

        public Task SignInAsync(ApplicationUser user, AuthenticationProperties properties, string authenticationMethod = null)
        {
            return _signInManager.SignInAsync(user, properties, authenticationMethod);
        }

        public async Task<ApplicationUser> AddLoginAsync(ApplicationUser user, IEnumerable<Claim> claims, ExternalLoginInfo externalLoginInfo)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            await _userManager.AddLoginAsync(user, externalLoginInfo);

            var result = await _userManager.AddClaimsAsync(user, claims);

            if (result.Succeeded)
            {
                return user;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
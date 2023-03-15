using Microsoft.AspNetCore.Identity;

namespace AlunosApi.Services
{
    public class AuthenticateService : IAuthenticate
    {
        private readonly SignInManager<IdentityUser> _signiInManager;
        private readonly UserManager<IdentityUser> _userManager;


        public AuthenticateService(SignInManager<IdentityUser> signiInManager, UserManager<IdentityUser> userManager)
        {
            _signiInManager = signiInManager;
            _userManager = userManager;
        }

        public async Task<bool> Authenticate(string email, string password)
        {
            var result = await _signiInManager.PasswordSignInAsync(email, password, 
                false, lockoutOnFailure: false);

            return result.Succeeded;
        }

        public async Task Logout()
        {
            await _signiInManager.SignOutAsync();
        }

        public async Task<bool> RegisterUser(string email, string password)
        {
            var appUser = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(appUser, password);

            if (result.Succeeded)
            {
                await _signiInManager.SignInAsync(appUser, isPersistent: false); //o false dis que não quer que o usuario continue loga quando o browser for fechado
            }

            return result.Succeeded;
        }
    }
}

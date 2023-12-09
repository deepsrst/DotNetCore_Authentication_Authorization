using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public LogoutModel(SignInManager<IdentityUser> signInManager)
        {
            SignInManager = signInManager;
        }

        public SignInManager<IdentityUser> SignInManager { get; }

        public async Task<IActionResult>  OnPostAsync()
        {
            await SignInManager.SignOutAsync();
            return RedirectToPage("/Account/Login");

        }
    }
}

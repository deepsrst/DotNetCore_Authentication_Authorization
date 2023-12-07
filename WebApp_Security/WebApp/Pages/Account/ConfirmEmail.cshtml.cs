using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {

        [BindProperty]
        public string Message { get; set; } = string.Empty;
        public ConfirmEmailModel(UserManager<IdentityUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<IdentityUser> UserManager { get; }

        public async Task<IActionResult> OnGetAsync(string userid, string confirmationToken)
        {

            var user = await UserManager.FindByIdAsync(userid);

            if (user != null)
            {
                var emailConfirm = await UserManager.ConfirmEmailAsync(user, confirmationToken);
                if (emailConfirm.Succeeded)
                {
                    Message = "Email Confirmed. Now, you can login with the registered email.";
                    return Page();
                }

            }
           
                Message = "Failed to validate user.";

            return Page();
        }
    }
}

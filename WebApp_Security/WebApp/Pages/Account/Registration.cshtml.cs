using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Pages.Account
{
    public class RegistrationModel : PageModel
    {

        public RegistrationModel(UserManager<IdentityUser> userManager)
        {
            UserManager = userManager;
        }
        [BindProperty]
        public RegistrationViewModel RegistrationViewModel { get; set; } = new RegistrationViewModel();
        public UserManager<IdentityUser> UserManager { get; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = new IdentityUser()
            {
                Email = RegistrationViewModel.Email,
                UserName = RegistrationViewModel.Email
            };

            var result = await UserManager.CreateAsync(user, RegistrationViewModel.Password);

            if (result.Succeeded)
            {
                var confirmationToken = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                return Redirect(Url.PageLink(pageName: "/Account/ConfirmEmail", values: new { userid = user.Id, confirmationToken = confirmationToken }) ?? "");
                //return RedirectToPage("/Account/Login");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }

                return Page();
            }
        }
    }

    public class RegistrationViewModel
    {

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(dataType: DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}

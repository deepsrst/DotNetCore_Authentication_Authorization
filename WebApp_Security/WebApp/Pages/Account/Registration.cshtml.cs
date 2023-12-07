using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using WebApp.Services;

namespace WebApp.Pages.Account
{
    public class RegistrationModel : PageModel
    {
        private readonly IEmailService emailService;

        public RegistrationModel(UserManager<IdentityUser> userManager, IEmailService emailService)
        {
            UserManager = userManager;
            this.emailService = emailService;
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
                //return Redirect(Url.PageLink(pageName: "/Account/ConfirmEmail", values: new { userid = user.Id, confirmationToken = confirmationToken }) ?? "");
               var confirmationLink= (Url.PageLink(pageName: "/Account/ConfirmEmail", values: new { userid = user.Id, confirmationToken = confirmationToken }) ?? "");
              
                await  emailService.SendAsync("shresthadeepak61@gmail.com", user.Email,"Confirm Email", $"Click the given link in order to confirm the registration {confirmationLink}");
                //var message = new MailMessage("shresthadeepak61@gmail.com", user.Email,
                //    "Confirm Email"
                //    , $"Click the given link in order to confirm the registration {confirmationLink}");

                //using (var emailClient = new SmtpClient("smtp-relay.brevo.com",587))
                //{
                //    emailClient.Credentials = new NetworkCredential("shresthadeepak61@gmail.com", "4BHXN2VMAPRO3DcT");

                //    await emailClient.SendMailAsync(message);
                return RedirectToAction("/Account/Login");

                //}

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

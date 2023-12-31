using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;

namespace WebApp_UnderTheHood.Pages.Account
{

    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; }  = new Credential();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync() 
        {
            if (!ModelState.IsValid) return Page();


            //verify the credential
            if(Credential.UserName=="admin" && Credential.Password=="admin")
            {
                // Creating the security context
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Email,"admin@myweb.com"),
                new Claim("Department","HR"),
                new Claim("Admin","true"),
                new Claim("Manager","true"),
                new Claim("EmploymentDate","2023-05-01")

            };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties() { IsPersistent= Credential.RememberMe };


                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);

                return RedirectToPage("/Index");

            }
            return Page();
        }
    }

    public class Credential
    {
        [Required]
        [Display(Name ="User Name")]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }
    }

}

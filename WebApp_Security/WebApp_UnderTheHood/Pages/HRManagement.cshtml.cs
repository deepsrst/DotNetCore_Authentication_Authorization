using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_UnderTheHood.Pages
{
    [Authorize(Policy ="HRManager")]
    public class HRManagementModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}

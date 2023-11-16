using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp_UnderTheHood.DTO;

namespace WebApp_UnderTheHood.Pages
{
    [Authorize(Policy ="HRManager")]
    public class HRManagementModel : PageModel
    {

        [BindProperty]
         public List<WeatherForecasetDTO> weatherForecastItems { get; set;}
        public HRManagementModel(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public IHttpClientFactory httpClientFactory { get; }

        public async Task OnGetAsync()
        {
            var httpClient = httpClientFactory.CreateClient("OurWebAPI");
            weatherForecastItems =await httpClient.GetFromJsonAsync<List<WeatherForecasetDTO>>("WeatherForecast")?? new List<WeatherForecasetDTO>();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using WebApp_UnderTheHood.Authorization;
using WebApp_UnderTheHood.DTO;
using WebApp_UnderTheHood.Pages.Account;

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

            // Post auth credential to get access_token
            var res = await httpClient.PostAsJsonAsync("auth", new Credential
            {
                UserName = "admin",
                Password = "admin"
            });
            res.EnsureSuccessStatusCode();
            string strJwt = await res.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<JwtToken>(strJwt);


            //send obtained token along with header
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken ?? string.Empty);

            weatherForecastItems =await httpClient.GetFromJsonAsync<List<WeatherForecasetDTO>>("WeatherForecast")?? new List<WeatherForecasetDTO>();
        }
    }
}

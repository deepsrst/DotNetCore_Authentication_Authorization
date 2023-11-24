using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Net.Http;
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

            JwtToken jwtToken = new JwtToken();

            var sessionToken = HttpContext.Session.GetString("access_token")??"";

            if(string.IsNullOrWhiteSpace(sessionToken))
            {
                 jwtToken= await AuthenticateAndGetToken();
            }
            else
            {
                jwtToken = JsonConvert.DeserializeObject<JwtToken>(sessionToken) ?? new JwtToken() ;
            }

            if(string.IsNullOrWhiteSpace(jwtToken.AccessToken) || jwtToken.ExpiresAt< DateTime.UtcNow)
            {
                jwtToken = await AuthenticateAndGetToken();
            }


            //send obtained token along with header
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken?.AccessToken ?? string.Empty);

            weatherForecastItems =await httpClient.GetFromJsonAsync<List<WeatherForecasetDTO>>("WeatherForecast")?? new List<WeatherForecasetDTO>();
        }


        public async Task<JwtToken> AuthenticateAndGetToken()
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
            HttpContext.Session.SetString("access_token", strJwt);
            return JsonConvert.DeserializeObject<JwtToken>(strJwt)?? new JwtToken();

        }
    }
}

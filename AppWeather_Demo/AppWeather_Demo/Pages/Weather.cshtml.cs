using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AppWeather_Demo.Pages
{
    public class WeatherModel : PageModel
    {

        //constructor 
        public WeatherModel(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        public IHttpClientFactory HttpClientFactory { get; }

        public async Task OnGet() // get triggered when the index is called
        {
            var client = HttpClientFactory.CreateClient("OurWebAPI");

            var result = await client.GetFromJsonAsync<APIResponse>("quotes");

        }
    }


    public class Quote
    {
        public int id { get; set; }
        public string quote { get; set; }
        public string author { get; set; }
    }

    public class APIResponse
    {
        public List<Quote> quotes { get; set; }
        public int total { get; set; }
        public int skip { get; set; }
        public int limit { get; set; }
    }
}

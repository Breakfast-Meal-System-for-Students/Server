using Microsoft.AspNetCore.Mvc.Testing;

namespace BMS.TEST
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureClient(HttpClient client)
        {
            client.BaseAddress = new Uri("https://bms-fs-api.azurewebsites.net");
        }
    }


}

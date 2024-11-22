using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BMS.TEST;
using Newtonsoft.Json;
using Xunit;
using BMS.API;

public class BaseTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    protected readonly HttpClient _client;
    private static string? _token;

    public BaseTest(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    protected async Task<string> GetTokenAsync()
    {
        if (!string.IsNullOrEmpty(_token))
        {
            return _token;
        }

        var loginUrl = "https://bms-fs-api.azurewebsites.net/api/Auth/login";

        var payload = new
        {
            email = "user@gmail.com",
            password = "User123@"
        };

        var x = JsonConvert.SerializeObject(payload);
        var content = new StringContent(x, Encoding.UTF8, "application/json");


        var response = await _client.PostAsync(loginUrl, content);

        var responseBody = await response.Content.ReadAsStringAsync();

        // Nếu có lỗi, ném exception
        response.EnsureSuccessStatusCode();

        var responseJson = JsonConvert.DeserializeObject<dynamic>(responseBody);

        _token = responseJson.data.token.ToString();
        return _token;
    }


    protected async Task<HttpRequestMessage> CreateAuthenticatedRequest(HttpMethod method, string url, object? body = null)
    {
        var request = new HttpRequestMessage(method, url);
        var token = await GetTokenAsync();

        // Thêm header Authorization
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Thêm body nếu có
        if (body != null)
        {
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        }

        return request;
    }
}

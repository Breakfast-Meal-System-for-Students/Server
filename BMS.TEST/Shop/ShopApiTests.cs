using BMS.TEST;
using System.Net.Http;
using System.Threading.Tasks;
using BMS.API;
using Xunit;

public class AccountApiTests : BaseTest
{
    public AccountApiTests(CustomWebApplicationFactory<Program> client) : base(client)
    {
    }

    [Fact]
    public async Task GetMyProfile_ShouldReturnOk()
    {
        // Tạo request với token
        var request = await CreateAuthenticatedRequest(HttpMethod.Get, "https://bms-fs-api.azurewebsites.net/api/Account/my-profile");

        // Gửi request
        var response = await _client.SendAsync(request);

        // Kiểm tra kết quả
        response.EnsureSuccessStatusCode();
        var responseData = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseData);
    }

    [Theory]
    [InlineData("170695d2-2d0b-435e-994d-08d0cf5525b0")]
    public async Task GetAccountById_ShouldReturnOk(string id)
    {
        // Tạo request với token
        var request = await CreateAuthenticatedRequest(HttpMethod.Get, $"https://bms-fs-api.azurewebsites.net/api/Account/{id}");

        // Gửi request
        var response = await _client.SendAsync(request);

        // Kiểm tra kết quả
        response.EnsureSuccessStatusCode();
        var responseData = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseData);
    }
}

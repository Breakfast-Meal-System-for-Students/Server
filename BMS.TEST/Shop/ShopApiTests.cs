using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BMS.BLL.Models;
using BMS.BLL.Models.Responses.Shop;

namespace BMS.TEST.Shop
{
    public class ShopApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ShopApiTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.UseSetting("https://localhost/", "7039");
            }).CreateClient();
        }

        [Fact]
        public async Task GetShopById_ShouldReturnOk()
        {
            Guid shopId = Guid.Parse("170695d2-2d0b-435e-9d94-08dcd0f525b0");

            var response = await _client.GetAsync($"/api/Shop/{shopId}");
            var responseData = await response.Content.ReadFromJsonAsync<ServiceActionResult>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseData);
            Assert.Equal(bool.TrueString, responseData.IsSuccess.ToString());
            Assert.Equal(shopId, ((ShopResponse)responseData.Data).Id);
        }
    }
}

using BMS.BLL.Models.Responses.AI;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class ProductAIDetectService : IProductAIDetectService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ProductAIDetectService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        // Core Detect 
        public async Task<string> DescribeImageAsync(IFormFile image, string des)
        {
            if (image == null || image.Length == 0)
                throw new ArgumentException("Invalid image file.");

            try
            {
                // Chuyển đổi ảnh sang chuỗi base64
                string base64Image;
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var imageBytes = memoryStream.ToArray();
                    base64Image = Convert.ToBase64String(imageBytes);
                }

                // Cấu hình thông tin API
                var endpoint = _configuration["GeminiAI:Endpoint"];
                var apiKey = _configuration["GeminiAI:ApiKey"];

                // Chuẩn bị dữ liệu JSON cho request
                var requestBody = new
                {
                    model = "gemini-1.5-flash",
                    generation_config = new
                    {
                        response_mime_type = "application/json"
                    },
                    contents = new[]
        {
        new
        {
            parts = new object[] // Khai báo kiểu rõ ràng cho mảng
            {
                new { text = des },
                new
                {
                    inline_data = new
                    {
                        mime_type = "image/png",
                        data = base64Image
                    }
                }
            }
        }
    }
                };

                var jsonContent = JsonSerializer.Serialize(requestBody);

                // Tạo HTTP client và gửi request
                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{endpoint}?key={apiKey}"),
                    Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                };

                var response = await client.SendAsync(request);

                // Xử lý kết quả trả về
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error from Gemini AI: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                // Parse JSON để trích xuất text
                var jsonObject = JsonNode.Parse(responseContent);
                var text = jsonObject?["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

                if (string.IsNullOrEmpty(text))
                    throw new Exception("Failed to extract 'text' from Gemini AI response.");

                return text;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing image: {ex.Message}", ex);
            }

        }

        // Check Validate vilates policies
        public async Task<string> PolicyImageAsync(IFormFile image)
        {
            if (image == null || image.Length == 0)
                throw new ArgumentException("Invalid image file.");

            try
            {

                string text = "If the image violates policies regarding nudity, violence, or political content and  the image is not related to the topic of food and beverages, return result= 0. If the image is valid, return result= 1. Use the following format exactly ,Loại bỏ Markdown ```json: {{\"result\": \"1 or 0\", \"reason\": \"this is the reason\"}}. For example: {{\"result\": \"1\", \"reason\": \"The image contains explicit content\"}}.";
                string result = await DescribeImageAsync(image, text);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing image: {ex.Message}", ex);
            }
        }
        // Detect match Image with name
        public async Task<string> DetectImageAsync(IFormFile image, string name)
        {
            if (image == null || image.Length == 0)
                throw new ArgumentException("Invalid image file.");

            try
            {

                string text = "If the image is related to the topic of food and beverages, check if it aligns with the name " + name + " If have return result= 1. If it does not match, return result= 0. Use the following format exactly ,Loại bỏ Markdown ```json: {{\"result\": \"1 or 0\", \"reason\": \"this is the reason\"}}. For example: {{\"result\": \"1\", \"reason\": \"The image does not match the name , or it is not related to food and beverages\"}}.";
                string result = await DescribeImageAsync(image, text);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing image: {ex.Message}", ex);
            }
        }

        //Detect match Image with Name (convert to Json)
        public async Task<ImageAIResponse> DetectImageProductAsync(IFormFile image, string name)
        {
            if (image == null || image.Length == 0)
                throw new ArgumentException("Invalid image file.");

            try
            {

                string content = await DetectImageAsync(image, name);
                ImageAIResponse result =  GetResultAndReasonFromJson(content);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing image: {ex.Message}", ex);
            }
        }

        //Convert Result to Json
        public ImageAIResponse GetResultAndReasonFromJson(string jsonString)
        {
            try
            {
                // Parse JSON
                var jsonObject = JsonDocument.Parse(jsonString);

                // Lấy giá trị của thuộc tính "result" và "reason"
                string result = jsonObject.RootElement.GetProperty("result").GetString();
                string reason = jsonObject.RootElement.GetProperty("reason").GetString();

                // Trả về một đối tượng ImageAIResponse
                return new ImageAIResponse
                {
                    Result = result,
                    Reason = reason
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error parsing JSON: {ex.Message}");
            }

        } 
    }
}

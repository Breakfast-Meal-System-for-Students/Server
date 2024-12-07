using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Users;
using BMS.BLL.Models;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace BMS.API.Controllers
{

    public class AIDetectController : ControllerBase
    {

        private readonly IProductAIDetectService _productAIDetectService;

        public AIDetectController(IProductAIDetectService productAIDetectService)
        {
            _productAIDetectService = productAIDetectService;
        }

        [HttpPost("DetectImagewithProductName")]
        public async Task<IActionResult> DescribeImage( IFormFile image , string productName)
        {
            try
            {
                var result = await _productAIDetectService.DetectImageAsync(image, productName);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("DetectFeedback")]
        public async Task<IActionResult> DetectFeedback(string feedback)
        {
            try
            {
                var result = await _productAIDetectService.AnalyzeFeedbackAsync(feedback);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}

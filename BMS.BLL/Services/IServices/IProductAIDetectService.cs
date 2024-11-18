using BMS.BLL.Models.Responses.AI;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
    public interface IProductAIDetectService
    {
        Task<string> DescribeImageAsync(IFormFile image, string text);
        Task<string> PolicyImageAsync(IFormFile image);
        Task<string> DetectImageAsync(IFormFile image, string name, string description);
        Task<ImageAIResponse> DetectImageProductAsync(IFormFile image, string name);
    }
}

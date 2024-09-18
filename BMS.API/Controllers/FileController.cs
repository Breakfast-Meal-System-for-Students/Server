using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    public class FileController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;

        public FileController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            var url = await _fileStorageService.UploadFileBlobAsync(file);
            return Ok(url);
        }
        [HttpPost("upload/test")]
        public async Task<IActionResult> UploadFileTest([FromForm] IFormFile file)
        {
            var url = await _fileStorageService.UploadFileBlobAsync(file);
            return Ok(url);
        }
    }
}

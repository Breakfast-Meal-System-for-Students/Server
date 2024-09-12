using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
    public interface IFileStorageService
    {
        Task<IEnumerable<string>> UploadFilesBlobAsync(List<IFormFile> files);
        Task<string> UploadFileBlobAsync(IFormFile file);
    }
}

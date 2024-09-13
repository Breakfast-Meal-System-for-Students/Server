using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Category
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; } = null!;
        public IFormFile? Image { get; set; }
        public string Description { get; set; } = null!;
    }
}

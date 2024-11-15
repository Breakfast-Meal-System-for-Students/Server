using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.Cart
{
    public class LinkResponse
    {
        public Guid CartId { get; set; }
        public string AccessToken { get; set; }
    }
}

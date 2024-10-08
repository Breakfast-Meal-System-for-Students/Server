using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Map
{
    public class RouteRequest
    {
        public List<Location> Origins { get; set; }
        public List<Location> Destinations { get; set; }
    }
    public class OriginDestination
    {
        public string address { get; set; }
    }
    public class Location
    {
        public string Address { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.Map
{
    public class RouteResponse
    {
        public List<Route> routes { get; set; }
    }

    public class Route
    {
        public int distanceMeters { get; set; }
        public string duration { get; set; }
    }
}

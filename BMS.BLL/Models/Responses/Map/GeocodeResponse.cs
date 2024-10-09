using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BMS.BLL.Models.Responses.Map
{
    public class GeocodeResponse
    {
        [JsonPropertyName("results")]
        public List<GeocodeResult> Results { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class GeocodeResult
    {
        [JsonPropertyName("address_components")]
        public List<AddressComponent> AddressComponents { get; set; }

        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; }

        [JsonPropertyName("formatted_address")]
        public string FormattedAddress { get; set; }
    }

    public class AddressComponent
    {
        [JsonPropertyName("long_name")]
        public string LongName { get; set; }

        [JsonPropertyName("short_name")]
        public string ShortName { get; set; }

        [JsonPropertyName("types")]
        public List<string> Types { get; set; }
    }

    public class Geometry
    {
        [JsonPropertyName("location")]
        public Location Location { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }
    }



}

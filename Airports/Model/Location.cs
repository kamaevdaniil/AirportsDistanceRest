using Newtonsoft.Json;

namespace Airports.Model
{
    public class Location
    {
        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }
    }
}

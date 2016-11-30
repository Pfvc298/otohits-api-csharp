using Newtonsoft.Json;

namespace Otohits.API.Entities
{
    public class Referer
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "percentage")]
        public int Percentage { get; set; }
    }
}

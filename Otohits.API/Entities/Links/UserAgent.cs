using Newtonsoft.Json;

namespace Otohits.API.Entities
{
    public class UserAgent
    {
        [JsonProperty(PropertyName = "percentage")]
        public int Percentage { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}

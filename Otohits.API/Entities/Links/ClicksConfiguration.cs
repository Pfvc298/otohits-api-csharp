using Newtonsoft.Json;

namespace Otohits.API.Entities
{
    public class ClicksConfiguration
    {
        [JsonProperty(PropertyName = "enable_clicks")]
        public bool EnableClicks { get; set; }
        [JsonProperty(PropertyName = "clicks_chance")]
        public int ClicksChance { get; set; }
        [JsonProperty(PropertyName = "max_clicks")]
        public int MaxClicks { get; set; }
        [JsonProperty(PropertyName = "min_clicks")]
        public int MinClicks { get; set; }
        [JsonProperty(PropertyName = "waiting_time")]
        public int WaitingTime { get; set; }
    }
}

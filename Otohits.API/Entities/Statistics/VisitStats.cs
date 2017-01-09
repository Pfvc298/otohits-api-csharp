using Newtonsoft.Json;

namespace Otohits.API.Entities
{
    public class VisitStat
    {
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }
        [JsonProperty(PropertyName = "hits")]
        public long Hits { get; set; }
    }
}

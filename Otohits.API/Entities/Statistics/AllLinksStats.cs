using Newtonsoft.Json;

namespace Otohits.API.Entities
{
    public class AllLinksStats : VisitsStats
    {
        [JsonProperty(PropertyName = "links_count")]
        public int LinksCount { get; set; }
    }
}

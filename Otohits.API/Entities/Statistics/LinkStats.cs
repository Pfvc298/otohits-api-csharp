using Newtonsoft.Json;

namespace Otohits.API.Entities
{
    public class LinkStats : VisitsStats
    {
        [JsonProperty(PropertyName = "link_id")]
        public int LinkId { get; set; }
    }
}

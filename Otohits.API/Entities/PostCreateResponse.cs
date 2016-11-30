using Newtonsoft.Json;

namespace Otohits.API.Entities
{
    public class PostCreateResponse
    {
        [JsonProperty( PropertyName = "id")]
        public long Id { get; set; }
    }
}

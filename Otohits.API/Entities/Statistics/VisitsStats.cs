using Newtonsoft.Json;
using System.Collections.Generic;

namespace Otohits.API.Entities
{
    public class VisitsStats
    {
        [JsonProperty(PropertyName = "visits")]
        public List<VisitStat> Visits { get; set; } = new List<VisitStat>();
    }
}

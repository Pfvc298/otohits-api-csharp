using Newtonsoft.Json;
using System.Collections.Generic;

namespace Otohits.API.Entities
{
    public class Throttling
    {
        [JsonProperty(PropertyName = "visits_per_hour")]
        public int VisitsPerHour { get; set; }
        [JsonProperty(PropertyName = "throttling_plan")]
        public ThrottlingPlan ThrottlingPlan { get; set; }
        [JsonProperty(PropertyName = "timezone")]
        public string Timezone { get; set; }
    }

    public class ThrottlingPlan : Dictionary<int, ThrottlingHourPlan>
    {
        public ThrottlingPlan()
        {
        }

        public ThrottlingPlan(int defaultValue)
        {
            for (int i = 0; i < 24; i++)
                Add(i, new ThrottlingHourPlan { Min = defaultValue, Max = defaultValue });
        }
    }

    public class ThrottlingHourPlan
    {
        [JsonProperty(PropertyName = "min")]
        public int Min { get; set; }
        [JsonProperty(PropertyName = "max")]
        public int Max { get; set; }
    }
}

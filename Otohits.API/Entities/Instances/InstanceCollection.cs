using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Otohits.API.Entities
{
    public class InstanceCollection
    {
        [JsonProperty( PropertyName = "running_instances")]
        public List<RunningInstance> RunningInstances { get; set; }
        [JsonProperty(PropertyName = "unavailable_instances")]
        public List<UnavailableInstance> UnavailableInstances { get; set; }
    }

    public class RunningInstance
    {
        [JsonProperty(PropertyName = "points")]
        public int Points { get; set; }
        [JsonProperty(PropertyName = "last_visit")]
        public DateTime LastVisit { get; set; }
        [JsonProperty(PropertyName = "ip")]
        public string IP { get; set; }
        [JsonProperty(PropertyName = "is_cheating")]
        public bool IsCheating { get; set; }
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "is_application")]
        public bool IsApplication { get; set; }
    }

    public class UnavailableInstance
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "ip")]
        public string IP { get; set; }
    }
}

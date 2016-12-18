using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Otohits.API.Entities
{
    public class User
    {

        [JsonProperty( PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty( PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty( PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty( PropertyName = "points")]
        public decimal Points { get; set; }
        [JsonProperty( PropertyName = "all_time_hits")]
        public decimal AllTimeHits { get; set; }
        [JsonProperty( PropertyName = "ratio")]
        public int Ratio { get; set; }
        [JsonProperty( PropertyName = "creation_date")]
        public DateTime CreationDate { get; set; }
        [JsonProperty( PropertyName = "auto_attribute")]
        public bool AutoAttribute { get; set; }
        [JsonProperty( PropertyName = "need_help")]
        public bool NeedHelp { get; set; }
        [JsonProperty( PropertyName = "subscriptions")]
        public List<string> Subscriptions { get; set; } = new List<string>();
        [JsonProperty( PropertyName = "categories")]
        public List<string> Categories { get; set; } = new List<string>();
    }
}

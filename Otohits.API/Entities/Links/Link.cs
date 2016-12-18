using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Otohits.API.Entities
{
    public class Link
    {
        [JsonProperty( PropertyName = "id")]
        public int Id { get; internal set; }
        [JsonProperty( PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty( PropertyName = "category")]
        public string Category { get; set; }

        [JsonProperty( PropertyName = "timer")]
        public int Timer { get; set; }
        [JsonProperty( PropertyName = "timer_max")]
        public int TimerMax { get; set; }

        [JsonProperty( PropertyName = "app_only")]
        public bool AppOnly { get; set; }
        [JsonProperty( PropertyName = "hide_referer")]
        public bool HideReferer { get; set; }
        [JsonProperty( PropertyName = "can_scroll")]
        public bool CanScroll { get; set; }
        [JsonProperty( PropertyName = "clicks")]
        public ClicksConfiguration Clicks { get; set; }

        [JsonProperty( PropertyName = "created_date")]
        public DateTime CreatedDate { get; internal set; }
        [JsonProperty( PropertyName = "status")]
        public string Status { get; internal set; }
        [JsonProperty( PropertyName = "last_hit_date")]
        public DateTime LastHitDate { get; internal set; }
        [JsonProperty( PropertyName = "points")]
        public decimal Points { get; internal set; }
        [JsonProperty( PropertyName = "hits")]
        public int Hits { get; internal set; }

        [JsonProperty( PropertyName = "override_referer")]
        public bool OverrideReferer { get; internal set; }
        [JsonProperty( PropertyName = "override_useragent")]
        public bool OverrideUserAgent { get; internal set; }
        [JsonProperty( PropertyName = "limit_visits")]
        public bool LimitVisits { get; internal set; }

        [JsonProperty( PropertyName = "referers")]
        public List<Referer> Referers { get; set; }
        [JsonProperty( PropertyName = "useragents")]
        public List<UserAgent> UserAgents { get; set; }
        [JsonProperty( PropertyName = "throttling")]
        public Throttling Throttling { get; set; }
        [JsonProperty( PropertyName = "countries")]
        public List<string> Countries { get; set; }
    }
}

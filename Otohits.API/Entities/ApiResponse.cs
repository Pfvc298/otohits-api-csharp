﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Otohits.API.Entities
{
    public class ApiResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool IsSuccess { get; set; }
        [JsonProperty(PropertyName = "errors")]
        public List<ApiError> Errors { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }
    }

    public class ApiError
    {
        [JsonProperty(PropertyName = "field")]
        public string Field { get; set; }
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
}

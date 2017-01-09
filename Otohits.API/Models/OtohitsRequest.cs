﻿using Newtonsoft.Json;
using Otohits.API.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Otohits.API
{
    /// <summary>
    /// http://docs.otohitsapi.apiary.io/
    /// </summary>
    public class OtohitsRequest
    {
        public const string ApiUrl = "https://api.otohits.net";

        private static OtohitsApiCredentials Credentials { get; set; }

        #region Request/Security methods
        /// <summary>
        /// Set the credentials for all the Otohits requests in this session
        /// </summary>
        /// <param name="credentials">ApiCredentials object (Otohits API key and Secret)</param>
        public static void SetCredentials(OtohitsApiCredentials credentials)
        {
            Credentials = credentials;
        }

        /// <summary>
        /// Set the credentials for all the Otohits requests in this session
        /// </summary>
        /// <param name="key">Otohits API Key</param>
        /// <param name="secret">Otohits API Secret</param>
        public static void SetCredentials(string key, string secret)
        {
            SetCredentials(new OtohitsApiCredentials { ApiKey = key, ApiSecret = secret });
        }

        /// <summary>
        /// Create HMAC-256 token to use with the API
        /// </summary>
        /// <param name="valueToEncode">Value to encode</param>
        /// <param name="secret">API Secret</param>
        /// <returns>Base64 token</returns>
        private string CreateToken(string valueToEncode, string secret)
        {
            secret = secret ?? "";
            var encoding = new ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(valueToEncode);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        /// <summary>
        /// Get the API Authorization header value (+token)
        /// </summary>
        /// <param name="path">Relative url to request</param>
        /// <param name="method">HTTP method used</param>
        /// <returns>Otohits API Authorization token value</returns>
        private string GetAuthorizationHeaderValue(string path, string method)
        {
            var timestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var headerValue = $"{Credentials.ApiKey}:{method.ToUpper()}:{Uri.EscapeUriString(path)}:{timestamp}";
            var token = CreateToken(headerValue, Credentials.ApiSecret);
            return $"{headerValue}:{token}";
        }

        /// <summary>
        /// Create an AuthenticationHeaderValue for easier further manipulation
        /// </summary>
        /// <param name="path">Relative url to request</param>
        /// <param name="method">HTTP method used</param>
        /// <returns>API Authorization header</returns>
        private AuthenticationHeaderValue GetAuthorizationHeader(string path, string method)
        {
            return new AuthenticationHeaderValue("otoapi", GetAuthorizationHeaderValue(path, method));
        }

        /// <summary>
        /// Make sure the URL is absolute (contains the API domain)
        /// </summary>
        /// <param name="path">Relative url to request</param>
        /// <returns>Absolute Url</returns>
        private string GetApiUrl(string path)
        {
            return path.StartsWith(ApiUrl) ? path : ApiUrl + path;
        }

        #endregion

        #region GET
        /// <summary>
        /// Basic GET request on the API, can be used to retreive any service result as json string
        /// </summary>
        /// <param name="path">Relative url to request</param>
        /// <returns>Json string of the result</returns>
        public string Get(string path)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetApiUrl(path));
                request.ContentType = "application/json";
                request.Headers.Add(HttpRequestHeader.Authorization, GetAuthorizationHeader(path, "GET").ToString());
                using (var response = request.GetResponse())
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                using (var er = ex.Response.GetResponseStream())
                using (var erRs = new StreamReader(er))
                {
                    return erRs.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// GET data from the API
        /// </summary>
        /// <typeparam name="T">Type of object to get back from the API</typeparam>
        /// <param name="path">Relative url to request</param>
        /// <returns>Typed object response</returns>
        public T Get<T>(string path)
        {
            var requestResult = Get(path);
            return JsonConvert.DeserializeObject<T>(requestResult);
        }

        /// <summary>
        /// Basic GET request on the API - async, can be used to retreive any service result as json string
        /// </summary>
        /// <param name="path">Relative url to request</param>
        /// <returns>Json string of the result</returns>
        public async Task<string> GetAsync(string path)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetApiUrl(path));
                request.ContentType = "application/json";
                request.Headers.Add(HttpRequestHeader.Authorization, GetAuthorizationHeader(path, "GET").ToString());
                using (var response = await request.GetResponseAsync().ConfigureAwait(false))
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                using (var er = ex.Response.GetResponseStream())
                using (var erRs = new StreamReader(er))
                {
                    return erRs.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// GET data from the API - async
        /// </summary>
        /// <typeparam name="T">Type of object to get back from the API</typeparam>
        /// <param name="path">Relative url to request</param>
        /// <returns>Typed object response</returns>
        public async Task<T> GetAsync<T>(string path)
        {
            var requestResult = await GetAsync(path).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(requestResult);
        }
        #endregion

        #region POST
        /// <summary>
        /// Basic POST request on the API, can be used to retreive any service result as json string
        /// </summary>
        /// <param name="path">Relative url to request</param>
        /// <param name="data">Data to send to the service</param>
        /// <returns>Json string of the result</returns>
        public string Post(string path, string data)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetApiUrl(path));
                request.ContentType = "application/json";
                request.Method = "POST";
                request.Headers.Add(HttpRequestHeader.Authorization, GetAuthorizationHeader(path, "POST").ToString());
                request.ContentLength = data.Length;
                using (var sr = new StreamWriter(request.GetRequestStream()))
                {
                    sr.Write(data);
                }

                using (var response = request.GetResponse())
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                using (var er = ex.Response.GetResponseStream())
                using (var erRs = new StreamReader(er))
                {
                    return erRs.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// POST data to the API - async
        /// </summary>
        /// <param name="path">Relative url to request</param>
        /// <param name="data">Data to send to the service</param>
        /// <returns>Typed object response</returns>
        public T Post<T>(string path, object data)
        {
            var requestResult = Post(path, JsonConvert.SerializeObject(data));
            return JsonConvert.DeserializeObject<T>(requestResult);
        }

        /// <summary>
        /// POST data to the API - async
        /// </summary>
        /// <typeparam name="T">Type of object to get back from the API</typeparam>
        /// <param name="path">Relative url to request</param>
        /// <param name="data">Data to send to the service</param>
        /// <returns>Json string of the result</returns>
        public async Task<string> PostAsync(string path, string data)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetApiUrl(path));
                request.ContentType = "application/json";
                request.Method = "POST";
                request.Headers.Add(HttpRequestHeader.Authorization, GetAuthorizationHeader(path, "POST").ToString());
                request.ContentLength = data.Length;

                using (var sr = new StreamWriter(await request.GetRequestStreamAsync().ConfigureAwait(false)))
                {
                    sr.Write(data);
                }

                using (var response = await request.GetResponseAsync().ConfigureAwait(false))
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                using (var er = ex.Response.GetResponseStream())
                using (var erRs = new StreamReader(er))
                {
                    return erRs.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// POST data to the API - async
        /// </summary>
        /// <typeparam name="T">Type of object to get back from the API</typeparam>
        /// <param name="path">Relative url to request</param>
        /// <param name="data">Data to send to the service</param>
        /// <returns>Typed object response</returns>
        public async Task<T> PostAsync<T>(string path, string data)
        {
            var requestResult = await PostAsync(path, data).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(requestResult);
        }
        #endregion

        #region PUT
        /// <summary>
        /// Basic PUT request on the API, can be used to retreive any service result as json string
        /// </summary>
        /// <param name="path">Relative url to request</param>
        /// <param name="data">Data to send to the service</param>
        /// <returns>Json string of the result</returns>
        public string Put(string path, string data)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetApiUrl(path));
                request.ContentType = "application/json";
                request.Method = "PUT";
                request.Headers.Add(HttpRequestHeader.Authorization, GetAuthorizationHeader(path, "PUT").ToString());
                request.ContentLength = data.Length;
                using (var sr = new StreamWriter(request.GetRequestStream()))
                {
                    sr.Write(data);
                }

                using (var response = request.GetResponse())
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                using (var er = ex.Response.GetResponseStream())
                using (var erRs = new StreamReader(er))
                {
                    return erRs.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// PUT data to the API - async
        /// </summary>
        /// <param name="path">Relative url to request</param>
        /// <param name="data">Data to send to the service</param>
        /// <returns>Typed object response</returns>
        public T Put<T>(string path, object data)
        {
            var requestResult = Put(path, JsonConvert.SerializeObject(data));
            return JsonConvert.DeserializeObject<T>(requestResult);
        }

        /// <summary>
        /// PUT data to the API - async
        /// </summary>
        /// <typeparam name="T">Type of object to get back from the API</typeparam>
        /// <param name="path">Relative url to request</param>
        /// <param name="data">Data to send to the service</param>
        /// <returns>Json string of the result</returns>
        public async Task<string> PutAsync(string path, string data)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetApiUrl(path));
                request.ContentType = "application/json";
                request.Method = "PUT";
                request.Headers.Add(HttpRequestHeader.Authorization, GetAuthorizationHeader(path, "PUT").ToString());
                request.ContentLength = data.Length;

                using (var sr = new StreamWriter(await request.GetRequestStreamAsync().ConfigureAwait(false)))
                {
                    sr.Write(data);
                }

                using (var response = await request.GetResponseAsync().ConfigureAwait(false))
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                using (var er = ex.Response.GetResponseStream())
                using (var erRs = new StreamReader(er))
                {
                    return erRs.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// PUT data to the API - async
        /// </summary>
        /// <typeparam name="T">Type of object to get back from the API</typeparam>
        /// <param name="path">Relative url to request</param>
        /// <param name="data">Data to send to the service</param>
        /// <returns>Typed object response</returns>
        public async Task<T> PutAsync<T>(string path, string data)
        {
            var requestResult = await PutAsync(path, data).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(requestResult);
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Basic DELETE request on the API, can be used to retreive any service result as json string
        /// </summary>
        /// <param name="path">Relative url to request</param>
        /// <param name="data">Data to send to the service</param>
        /// <returns>Json string of the result</returns>
        public string Delete(string path)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetApiUrl(path));
                request.ContentType = "application/json";
                request.Method = "DELETE";
                request.Headers.Add(HttpRequestHeader.Authorization, GetAuthorizationHeader(path, "DELETE").ToString());

                using (var response = request.GetResponse())
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    string result = sr.ReadToEnd();
                    //Delete response (204) returns no content, creating a basic ApiResponse if it's the case
                    if (string.IsNullOrEmpty(result))
                        return JsonConvert.SerializeObject(new ApiResponse { IsSuccess = true });
                    return result;
                }
            }
            catch (WebException ex)
            {
                using (var er = ex.Response.GetResponseStream())
                using (var erRs = new StreamReader(er))
                {
                    return erRs.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// DELETE data to the API - async
        /// </summary>
        /// <param name="path">Relative url to request</param>
        /// <param name="data">Data to send to the service</param>
        /// <returns>Typed object response</returns>
        public T Delete<T>(string path)
        {
            var requestResult = Delete(path);
            return JsonConvert.DeserializeObject<T>(requestResult);
        }

        /// <summary>
        /// DELETE data to the API - async
        /// </summary>
        /// <typeparam name="T">Type of object to get back from the API</typeparam>
        /// <param name="path">Relative url to request</param>
        /// <param name="data">Data to send to the service</param>
        /// <returns>Json string of the result</returns>
        public async Task<string> DeleteAsync(string path)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetApiUrl(path));
                request.ContentType = "application/json";
                request.Method = "DELETE";
                request.Headers.Add(HttpRequestHeader.Authorization, GetAuthorizationHeader(path, "DELETE").ToString());

                using (var response = await request.GetResponseAsync().ConfigureAwait(false))
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    string result = sr.ReadToEnd();
                    //Delete response (204) returns no content, creating a basic ApiResponse if it's the case
                    if (string.IsNullOrEmpty(result))
                        return JsonConvert.SerializeObject(new ApiResponse { IsSuccess = true });
                    return result;
                }
            }
            catch (WebException ex)
            {
                using (var er = ex.Response.GetResponseStream())
                using (var erRs = new StreamReader(er))
                {
                    return erRs.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// DELETE data to the API - async
        /// </summary>
        /// <typeparam name="T">Type of object to get back from the API</typeparam>
        /// <param name="path">Relative url to request</param>
        /// <param name="data">Data to send to the service</param>
        /// <returns>Typed object response</returns>
        public async Task<T> DeleteAsync<T>(string path)
        {
            var requestResult = await DeleteAsync(path).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(requestResult);
        }
        #endregion

        #region User mapping
        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/endpoints/user/get-me
        /// </summary>
        public ApiResponse<User> GetUserInfo()
        {
            var user = Get<ApiResponse<User>>("/me");
            return user;
        }
        #endregion

        #region Instances mapping
        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/endpoints/instances/get-instances
        /// </summary>
        public ApiResponse<InstanceCollection> GetInstances()
        {
            var instances = Get<ApiResponse<InstanceCollection>>("/instances");
            return instances;
        }
        #endregion

        #region Links mapping
        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/endpoints/links/get-links
        /// </summary>
        public ApiResponse<List<Link>> GetLinks(string with = "")
        {
            var links = Get<ApiResponse<List<Link>>>("/links" + (!string.IsNullOrEmpty(with) ? $"?with={with}" : ""));
            return links;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/endpoints/links/post-link
        /// </summary>
        public ApiResponse<PostCreateResponse> PostLink(Link link)
        {
            var postResult = Post<ApiResponse<PostCreateResponse>>("/links", link);
            return postResult;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/services/links/delete-link
        /// </summary>
        public ApiResponse DeleteLink(int linkId)
        {
            var postResult = Delete<ApiResponse>("/links/" + linkId);
            return postResult;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/services/links/put-timers
        /// </summary>
        public ApiResponse UpdateLinksTimers(LinksTimersRequestModel data)
        {
            var updateResponse = Put<ApiResponse>("/links/timers", data);
            return updateResponse;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/services/links/put-points
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ApiResponse AddPointsToLinks(LinksPointsRequestModel data)
        {
            var updateResponse = Put<ApiResponse>("/links/points", data);
            return updateResponse;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/services/links/put-geotargeting
        /// </summary>
        public ApiResponse UpdateLinksGeoTargeting(LinksGeoTargetingRequestModel data)
        {
            var updateResponse = Put<ApiResponse>("/links/geotargeting", data);
            return updateResponse;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/services/links/put-clicks
        /// </summary>
        public ApiResponse UpdateLinksClicks(LinksClicksRequestModel data)
        {
            var updateResponse = Put<ApiResponse>("/links/clicks", data);
            return updateResponse;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/services/links/put-scroll
        /// </summary>
        public ApiResponse UpdateLinksScroll(LinksSwitchRequestModel data)
        {
            var updateResponse = Put<ApiResponse>("/links/scroll", data);
            return updateResponse;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/services/links/put-apponly
        /// </summary>
        public ApiResponse UpdateLinksAppOnly(LinksSwitchRequestModel data)
        {
            var updateResponse = Put<ApiResponse>("/links/apponly", data);
            return updateResponse;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/services/links/put-user-agents
        /// </summary>
        public ApiResponse UpdateLinksUserAgents(LinksUserAgentsRequestModel data)
        {
            var updateResponse = Put<ApiResponse>("/links/useragents", data);
            return updateResponse;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/services/links/put-referers
        /// </summary>
        public ApiResponse UpdateLinksReferers(LinksReferersRequestModel data)
        {
            var updateResponse = Put<ApiResponse>("/links/referers", data);
            return updateResponse;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/services/links/put-throttling
        /// </summary>
        public ApiResponse UpdateLinksThrottling(LinksThrottlingRequestModel data)
        {
            var updateResponse = Put<ApiResponse>("/links/throttling", data);
            return updateResponse;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/services/links/put-pause-link
        /// </summary>
        public ApiResponse PauseLinks(LinksBaseRequestModel data)
        {
            var updateResponse = Put<ApiResponse>("/links/pause", data);
            return updateResponse;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/services/links/put-start-link
        /// </summary>
        public ApiResponse StartLinks(LinksBaseRequestModel data)
        {
            var updateResponse = Put<ApiResponse>("/links/start", data);
            return updateResponse;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/services/links/put-reclaim-points-on-link
        /// </summary>
        public ApiResponse ReclaimPointsOnLinks(LinksBaseRequestModel data)
        {
            var updateResponse = Put<ApiResponse>("/links/reclaim", data);
            return updateResponse;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/services/links/put-name
        /// </summary>
        public ApiResponse UpdateLinksName(LinksValueRequestModel data)
        {
            var updateResponse = Put<ApiResponse>("/links/name", data);
            return updateResponse;
        }
        #endregion

        #region Stats mapping
        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/services/statistics/get-all-links-stats
        /// </summary>
        public ApiResponse GetLinksStats(StatsPeriod? period = null)
        {
            var stats = Get<ApiResponse<AllLinksStats>>($"/links/stats{(period != null ? "/" + period.Value.ToString().ToLower() : "")}");
            return stats;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/services/statistics/get-link-stats
        /// </summary>
        public ApiResponse GetLinkStats(int linkId, StatsPeriod? period = null)
        {
            var stats = Get<ApiResponse<LinkStats>>($"/links/{linkId}/stats{(period != null ? "/" + period.Value.ToString().ToLower() : "")}");
            return stats;
        }
        #endregion
    }
}

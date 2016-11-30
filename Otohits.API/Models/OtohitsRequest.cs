using Newtonsoft.Json;
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
        public T Post<T>(string path, string data)
        {
            var requestResult = Post(path, data);
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
        /// GET data to the API - async
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

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/endpoints/links/get-links
        /// </summary>
        /// <param name="with"></param>
        /// <returns></returns>
        public ApiResponse<List<Link>> GetLinks(string with = "")
        {
            var links = Get<ApiResponse<List<Link>>>("/links" + (!string.IsNullOrEmpty(with) ? $"?with={with}" : ""));
            return links;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/endpoints/links/post-link
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public ApiResponse<PostCreateResponse> PostLink(Link link)
        {
            var postResult = Post<ApiResponse<PostCreateResponse>>("/links", JsonConvert.SerializeObject(link));
            return postResult;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/endpoints/user/get-me
        /// </summary>
        /// <returns>User info</returns>
        public ApiResponse<User> GetUserInfo()
        {
            var user = Get<ApiResponse<User>>("/me");
            return user;
        }

        /// <summary>
        /// http://docs.otohitsapi.apiary.io/#reference/endpoints/instances/get-instances
        /// </summary>
        /// <returns>Running and inactive instances</returns>
        public ApiResponse<InstanceCollection> GetInstances()
        {
            var instances = Get<ApiResponse<InstanceCollection>>("/instances");
            return instances;
        }
    }
}

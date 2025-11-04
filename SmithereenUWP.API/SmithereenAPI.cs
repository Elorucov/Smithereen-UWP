using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmithereenUWP.API.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace SmithereenUWP.API
{
    public sealed class SmithereenAPI
    {
        public const string VERSION = "1.0";
        public const int ERROR_UNKNOWN = -1;
        public const int ERROR_INVALID_RESPONSE = -2;
        public const int ERROR_DOMAIN_NOT_RESOLVED = -3;

        private readonly HttpClient _client;

        public string ServerURL { get; private set; }
        public string UserAgent { get; private set; }
        public string Language { get; set; } = "en-US";
        public string AccessToken { get; set; }

        #region Methods

        public ServerMethods Server { get; private set; }

        #endregion

        public SmithereenAPI(string serverUrl, string userAgent)
        {
            if (!Uri.IsWellFormedUriString($"https://{serverUrl}", UriKind.Absolute)) throw new ArgumentException($"{nameof(serverUrl)} is invalid!");

            _client = new HttpClient();
            _client.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue(userAgent));

            ServerURL = serverUrl;
            UserAgent = userAgent;

            Server = new ServerMethods(this);
        }

        public async Task<string> SendRequestAsync(string method, Dictionary<string, string> parameters = null)
        {
            var requestUri = new Uri($"https://{ServerURL}/api/method/{method}");
            if (parameters == null) parameters = new Dictionary<string, string>();

            if (!parameters.ContainsKey("v")) parameters.Add("v", VERSION);
            if (!parameters.ContainsKey("image_format")) parameters.Add("image_format", "jpeg"); // because old versions of UWP doesn't support WEBP.

            using (HttpRequestMessage hrm = new HttpRequestMessage(HttpMethod.Post, requestUri))
            {
                if (!string.IsNullOrEmpty(AccessToken)) hrm.Headers.Authorization = new HttpCredentialsHeaderValue("Bearer", AccessToken);
                hrm.Content = new HttpFormUrlEncodedContent(parameters);
                hrm.Headers.AcceptEncoding.Add(new HttpContentCodingWithQualityHeaderValue("UTF-8"));
                using (var response = await _client.SendRequestAsync(hrm))
                {
                    var buffer = await response.Content.ReadAsBufferAsync();
                    byte[] resultByteArray = buffer.ToArray();
                    return Encoding.UTF8.GetString(resultByteArray, 0, resultByteArray.Length);
                }
            }
        }

        public async Task<T> CallMethodAsync<T>(string method, Dictionary<string, string> parameters = null)
        {
            try
            {
                string response = await SendRequestAsync(method, parameters);

                JObject json = JObject.Parse(response);
                if (json["error"] != null)
                {
                    int code = json["error"]["error_code"].Value<int>();
                    string message = json["error"]["error_msg"].Value<string>();
                    throw new SmithereenAPIException(code, message);
                }
                else
                {
                    return json["response"].ToObject<T>();
                }
            } catch (JsonReaderException jrex)
            {
                throw new SmithereenAPIException(ERROR_INVALID_RESPONSE, "Expected JSON but non-JSON response received");
            } catch (COMException comex)
            {
                if (comex.Message.Contains("The server name or address could not be resolved"))
                    throw new SmithereenAPIException(ERROR_DOMAIN_NOT_RESOLVED, "Server unreachable");
                throw comex;
            }
        }
    }
}

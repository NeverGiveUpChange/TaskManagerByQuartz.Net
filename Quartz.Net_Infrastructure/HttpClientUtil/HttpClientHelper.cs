
using Newtonsoft.Json;
using Quartz.Net_Infrastructure.RsaCryptionUtil;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Quartz.Net_Infrastructure.HttpClientUtil
{
    public class HttpClientHelper
    {
        private static readonly HttpClient _httpClient;

        static HttpClientHelper() {
            _httpClient = new HttpClient() { BaseAddress = new Uri("https://www.baidu.com/") };
            _httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
            //帮HttpClient热身
            _httpClient.SendAsync(new HttpRequestMessage
            {
                Method = new HttpMethod("HEAD"),
                RequestUri = new Uri("https://www.baidu.com/")
            })
                .Result.EnsureSuccessStatusCode();
        }
        public async Task<string> PostAsync<T>(T postData,string requestUrl=null)
        {
             
            requestUrl = string.IsNullOrWhiteSpace(requestUrl) ? "http://localhost:52043/JobManager/UpdateJobInfo" : requestUrl;
            HttpContent content = new StringContent(JsonConvert.SerializeObject(postData));
            MediaTypeHeaderValue typeHeader = new MediaTypeHeaderValue("application/json");
            typeHeader.CharSet = "UTF-8";
            content.Headers.ContentType = typeHeader;
            var response = await _httpClient.PostAsync(new Uri( requestUrl), content);

            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> GetAsync(string requestUrl) {
            string baseUrl = string.Empty;
            var parametersDictionary = _parseUrl(requestUrl, out baseUrl);
            var SortedQueryString = _sortQueryString(parametersDictionary);
            _setHttpHeader(SortedQueryString);
            var response = await _httpClient.GetAsync(new Uri(string.Concat(baseUrl, "?", SortedQueryString)));
            return await response.Content.ReadAsStringAsync();
        }
        private Dictionary<string, string> _parseUrl(string url ,out string baseUrl) {
            baseUrl = string.Empty;
            if (string.IsNullOrWhiteSpace(url)) {
                return null;
            }
            int questionMarkIndex = url.IndexOf('?');

            if (questionMarkIndex == -1)
                baseUrl = url;
            else
                baseUrl = url.Substring(0, questionMarkIndex);
            if (questionMarkIndex == url.Length - 1)
                return null;
            string ps = url.Substring(questionMarkIndex + 1);
            Dictionary<string, string> parametersDictionary = new Dictionary<string, string>();
            Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
            MatchCollection mc = re.Matches(ps);
            foreach (Match m in mc)
            {
                parametersDictionary.Add(m.Result("$2").ToLower(), m.Result("$3"));
            }

            return parametersDictionary;
        }

        private string _sortQueryString(Dictionary<string,string> parametersDictionary) {
            string queryString = string.Empty;
            var requestDictionary = parametersDictionary.Where(x => !string.IsNullOrWhiteSpace(x.Value)).OrderBy(x => x.Key);
            if (requestDictionary.Any()) {
                queryString = string.Join("&", requestDictionary.Select(x => string.Format("{0}={1}",x.Key,x.Value.Trim())).ToArray());
            }
            return queryString;
        }
        private string _getSign(int expiry,string timestamp,  string requestParameters) {
            return $"expiry{expiry}timestamp{timestamp}{requestParameters}".SHA256().RSAEncrypt(Encoding.UTF8.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings["PlatformPrivateKey_Web"])));
        }
        private void _setHttpHeader(string requestParameters) {
            string timestamp = RsaCryptionHelper.GetTimeStmap();
            int expiry = 0;
            string platformtype = "Web";
            _httpClient.DefaultRequestHeaders.Add("platformtype", platformtype);
            _httpClient.DefaultRequestHeaders.Add("sign", $"{_getSign(expiry, timestamp, requestParameters)}");
            _httpClient.DefaultRequestHeaders.Add("timestamp", timestamp);
            _httpClient.DefaultRequestHeaders.Add("expiry", expiry.ToString());
        }

    }
}

using Newtonsoft.Json;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Lamtip
{
    public enum ParamType
    {
        Object,
        SimpleValue,
        String
    }

    /// <summary>
    /// 
    /// </summary>
    public class ApiHelper : IDisposable
    {

        public string BaseUrl { set; private get; }

        public HttpClient HttpClient { private set; get; }

        public ApiHelper(string host)
        {
            this.BaseUrl = host;
            this.HttpClient = new HttpClient()
            {
                BaseAddress = new Uri(host)
            };
        }

        ~ApiHelper()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            this.HttpClient?.Dispose();
            this.HttpClient = null;
        }



        public string GetApi(string controller, string action)
        {
            return $"{controller}/{action}";
        }

        /// <summary>
        /// 生成请求内容
        /// </summary>
        /// <param name="method"></param>
        /// <param name="urlParam"></param>
        /// <param name="requestBody"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private HttpRequestMessage MakeRequest(HttpMethod method, UrlParam urlParam, object requestBody, string controller, string action, ParamType type,
            Func<string, string, HttpRequestMessage, Dictionary<string, string>> makeHeader)
        {
            var url = this.GetApi(controller, action);
            var param = string.Join("&", (urlParam?.Select(x => $"{x.Key}={x.Value?.ToString()}") ?? Enumerable.Empty<string>()));
            if (!string.IsNullOrEmpty(param))
            {
                url = $"{url}?{param}";
            }

            var request = new HttpRequestMessage(method, url);
            if (requestBody != null)
            {
                request.Content = this.MakeParamContent(requestBody, type);
            }
            makeHeader?.Invoke(controller, action, request)?.ToList()?.ForEach(x =>
            {
                request.Headers.Add(x.Key, x.Value);
            });
            return request;
        }

        private HttpRequestMessage MakeRequest(HttpMethod method, string url, object body, IDictionary<string, string> headers)
        {
            var request = new HttpRequestMessage(method, url);
            if (body != null)
            {
                var isString = body.GetType() == typeof(string);
                request.Content = this.MakeParamContent(body, isString ? ParamType.String : ParamType.Object);
            }
            headers?.ToList()?.ForEach(v => request.Headers.Add(v.Key, v.Value));
            return request;
        }

        private HttpContent MakeParamContent(object requestBody, ParamType type)
        {
            var requestStr = requestBody?.GetType() == typeof(string) ? (string)requestBody : JsonConvert.SerializeObject(requestBody);
            if (string.IsNullOrEmpty(requestStr)) return null;
            HttpContent content = null;
            switch (type)
            {
                case ParamType.SimpleValue:
                    content = new FormUrlEncodedContent(new Dictionary<string, string> { { "",  requestStr } });
                    break;
                case ParamType.Object:
                    content = new StringContent(requestStr, Encoding.UTF8, "application/json");
                    break;
                case ParamType.String:
                    content = new StringContent(requestStr, Encoding.UTF8, "application/x-www-form-urlencoded");
                    break;
            }
            return content;
        }



        private T GetNormalValue<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ToString());
            }

            var data = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<T>(data);
            return result;
        }

        private string GetNormalValue(HttpResponseMessage response , bool isStream = false)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ToString());
            }

            
            var data = response.Content.ReadAsStringAsync().Result; 
            return data;
        }

        private Stream GetNormalStreamValue(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            var data = response.Content.ReadAsStreamAsync().Result;
            return data;
        }

        /// <summary>
        /// 发送数据并返回(非AjaxResult对象)
        /// </summary>
        /// <param name="method"></param>
        /// <param name="urlParam"></param>
        /// <param name="requestBody"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<string> SendNormal(HttpMethod method, UrlParam urlParam, object requestBody, string controller, string action, ParamType type = ParamType.SimpleValue, Func<string, string, HttpRequestMessage, Dictionary<string, string>> makeHeader = null)
        {
            var request = this.MakeRequest(method, urlParam, requestBody, controller, action, type, makeHeader);
            var response = await this.HttpClient.SendAsync(request);
            return this.GetNormalValue(response);

        }

        public async Task<Stream> SendStreamNormal(HttpMethod method, UrlParam urlParam, object requestBody, string controller, string action, ParamType type = ParamType.SimpleValue, Func<string, string, HttpRequestMessage, Dictionary<string, string>> makeHeader = null, bool isStream = false)
        {
            var request = this.MakeRequest(method, urlParam, requestBody, controller, action, type, makeHeader);
            var response = await this.HttpClient.SendAsync(request);
            return this.GetNormalStreamValue(response);

        }

        public async Task<string> GetNormal(string controller, string action, UrlParam urlParam = null, Func<string, string, HttpRequestMessage, Dictionary<string, string>> makeHeader = null)
        {
            return await this.SendNormal(HttpMethod.Get, urlParam, null, controller, action, makeHeader: makeHeader);
        }



        public async Task<string> PostNormal(string controller, string action, UrlParam urlParam = null, object requestBody = null, ParamType type = ParamType.SimpleValue, Func<string, string, HttpRequestMessage, Dictionary<string, string>> makeHeader = null)
        {
            return await this.SendNormal(HttpMethod.Post, urlParam, requestBody, controller, action, type, makeHeader);
        }

        public async Task<Stream> GetByStream(string controller, string action, UrlParam urlParam = null, object requestBody = null, ParamType type = ParamType.SimpleValue, Func<string, string, HttpRequestMessage, Dictionary<string, string>> makeHeader = null)
        {
            var request = this.MakeRequest(HttpMethod.Get, urlParam, requestBody, controller, action, type, makeHeader);
            var response = await this.HttpClient.SendAsync(request);
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<Stream> SendStram(HttpMethod method ,string controller, string action, UrlParam urlParam = null, object requestBody = null, ParamType type = ParamType.SimpleValue, Func<string, string, HttpRequestMessage, Dictionary<string, string>> makeHeader = null)
        {
            return await this.SendStreamNormal(method, urlParam, requestBody, controller, action, type, makeHeader);
        }




        public async Task<string> PutNormal(string controller, string action, UrlParam urlParam = null, object requestBody = null, ParamType type = ParamType.SimpleValue, Func<string, string, HttpRequestMessage, Dictionary<string, string>> makeHeader = null)
        {
            return await this.SendNormal(HttpMethod.Put, urlParam, requestBody, controller, action, type, makeHeader);
        }




        public async Task<string> DeleteNormal(string controller, string action, UrlParam urlParam = null, object requestBody = null, ParamType type = ParamType.SimpleValue, Func<string, string, HttpRequestMessage, Dictionary<string, string>> makeHeader = null)
        {
            return await this.SendNormal(HttpMethod.Delete, urlParam, requestBody, controller, action, type, makeHeader);
        }


    }

}

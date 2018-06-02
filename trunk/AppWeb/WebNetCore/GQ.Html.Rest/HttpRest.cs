using GQ.Core.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GQ.Html.Rest
{
    public class HttpRest
    {
        private Uri _baseAddress = null;

        public string BaseAddres { get { return _baseAddress == null ? "" : _baseAddress.AbsoluteUri; } set { _baseAddress = new Uri(value); } }

        public Dictionary<string, string> Cookies = new Dictionary<string, string>();

        public HttpStatusCode StatusCode { get; private set; }
        public bool IsSuccessStatusCode { get; private set; }
        public HttpRequestMessage RequestMessage { get; private set; }

        #region HTTP_GET

        public async Task<string> GetString(string url)
        {
            var uri = new Uri(url);

            return await GetString(uri);
        }

        public async Task<string> GetString(string url, string[] postParameters = null)
        {

            if (postParameters != null)
            {
                url = PrepareURL(url);

                StringBuilder postData = new StringBuilder();

                List<string> data = new List<string>();

                for (var i = 0; i < postParameters.Length; i++)
                {
                    data.Add(HttpUtility.UrlEncode(postParameters[i]));
                }

                postData.AppendJoin("/", data);

                var uri = new Uri(url + postData);

                return await GetString(uri);
            }
            else
                return await GetString(url);

        }

        public async Task<string> GetString(string url, Dictionary<string, string> postParameters = null)
        {
            if (postParameters != null)
            {
                url = PrepareURL(url);

                StringBuilder postData = new StringBuilder();

                foreach (string key in postParameters.Keys)
                {
                    postData.Append(HttpUtility.UrlEncode(key) + "="
                          + HttpUtility.UrlEncode(postParameters[key]) + "&");
                }

                var uri = new Uri(url + "?" + postData);

                return await GetString(uri);
            }
            else
                return await GetString(url);
        }

        private string PrepareURL(string url)
        {
            return url + (url.Substring(url.Length - 1, 1) == "/" ? "" : "/");
        }

        private async Task<string> GetString(Uri uri)
        {
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = _baseAddress })
            {
                foreach (var key in Cookies.Keys)
                {
                    cookieContainer.Add(_baseAddress, new Cookie(key, Cookies[key]));
                }

                var response = await client.GetAsync(uri);
                StatusCode = response.StatusCode;
                IsSuccessStatusCode = response.IsSuccessStatusCode;
                RequestMessage = response.RequestMessage;
                var read = await response.Content.ReadAsStringAsync();
                return read;
            }
        }

        private async Task<byte[]> GetByte(Uri uri)
        {
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = _baseAddress })
            {
                foreach (var key in Cookies.Keys)
                {
                    cookieContainer.Add(_baseAddress, new Cookie(key, Cookies[key]));
                }

                var response = await client.GetAsync(uri);
                StatusCode = response.StatusCode;
                IsSuccessStatusCode = response.IsSuccessStatusCode;
                RequestMessage = response.RequestMessage;
                var read = await response.Content.ReadAsByteArrayAsync();
                return read;
            }
        }

        private async Task<Stream> GetStream(Uri uri)
        {
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = _baseAddress })
            {
                foreach (var key in Cookies.Keys)
                {
                    cookieContainer.Add(_baseAddress, new Cookie(key, Cookies[key]));
                }

                var response = await client.GetAsync(uri);
                StatusCode = response.StatusCode;
                IsSuccessStatusCode = response.IsSuccessStatusCode;
                RequestMessage = response.RequestMessage;
                var read = await response.Content.ReadAsStreamAsync();
                return read;
            }
        }

        #endregion

        #region HTTP_POST

        public async Task<string> PostString(string url)
        {
            var uri = new Uri(url);

            return await PostString(uri, null);
        }

        public async Task<string> PostString(string url, object data)
        {
            var uri = new Uri(url);

            return await PostString(uri, data);
        }

        public async Task<byte[]> PostByte(string url)
        {
            var uri = new Uri(url);

            return await PostByte(uri, null);
        }

        public async Task<byte[]> PostByte(string url, object data)
        {
            var uri = new Uri(url);

            return await PostByte(uri, data);
        }

        public async Task<Stream> PostStream(string url)
        {
            var uri = new Uri(url);

            return await PostStream(uri, null);
        }

        public async Task<Stream> PostStream(string url, object data)
        {
            var uri = new Uri(url);

            return await PostStream(uri, data);
        }

        private async Task<string> PostString(Uri uri, object data)
        {
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = _baseAddress })
            {
                foreach (var key in Cookies.Keys)
                {
                    cookieContainer.Add(_baseAddress, new Cookie(key, Cookies[key]));
                }

                HttpContent content = new StringContent(data == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(uri, content);
                StatusCode = response.StatusCode;
                IsSuccessStatusCode = response.IsSuccessStatusCode;
                RequestMessage = response.RequestMessage;
                var read = await response.Content.ReadAsStringAsync();
                return read;
            }
        }

        private async Task<byte[]> PostByte(Uri uri, object data)
        {
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = _baseAddress })
            {
                foreach (var key in Cookies.Keys)
                {
                    cookieContainer.Add(_baseAddress, new Cookie(key, Cookies[key]));
                }

                HttpContent content = new StringContent(data == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(uri, content);
                StatusCode = response.StatusCode;
                IsSuccessStatusCode = response.IsSuccessStatusCode;
                RequestMessage = response.RequestMessage;
                var read = await response.Content.ReadAsByteArrayAsync();
                return read;
            }
        }

        private async Task<Stream> PostStream(Uri uri, object data)
        {
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = _baseAddress })
            {
                foreach (var key in Cookies.Keys)
                {
                    cookieContainer.Add(_baseAddress, new Cookie(key, Cookies[key]));
                }

                HttpContent content = new StringContent(data == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(uri, content);
                StatusCode = response.StatusCode;
                IsSuccessStatusCode = response.IsSuccessStatusCode;
                RequestMessage = response.RequestMessage;
                var read = await response.Content.ReadAsStreamAsync();
                return read;
            }
        }

        #endregion

    }
}

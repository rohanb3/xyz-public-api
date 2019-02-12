using System;
using System.Net;
using System.Text;

namespace Xyzies.TWC.OptymyzeClient.Client
{
    public abstract class BaseClient
    {
        /// <summary>
        /// GET
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="referer"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public HttpWebResponse Get(string requestUri, string referer = null, string cookies = null, string xBrowserId = null)
        {
            if (string.IsNullOrWhiteSpace(requestUri))
            {
                throw new ArgumentNullException("requestUri");
            }

            var request = WebRequest.Create(requestUri) as HttpWebRequest;
            request.Method = "GET";
            request.AllowAutoRedirect = false;

            AddHeaders(ref request, referer, cookies, xBrowserId);

            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="parameters"></param>
        /// <param name="referer"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public HttpWebResponse Post(string requestUri, string parameters, string referer = null, string cookies = null, string facesRequest = null, string xbrowserId = null)
        {
            if (string.IsNullOrWhiteSpace(requestUri) || string.IsNullOrWhiteSpace(parameters))
            {
                throw new ArgumentNullException("requestUri");
            }

            var request = WebRequest.Create(requestUri) as HttpWebRequest;

            var data = Encoding.UTF8.GetBytes(parameters);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.AllowAutoRedirect = false;

            request.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
            request.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            AddHeaders(ref request, referer, cookies, xbrowserId);
            if (!string.IsNullOrWhiteSpace(facesRequest))
            {
                request.Headers.Add("Faces-Request", facesRequest);
            }

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            return request.GetResponse() as HttpWebResponse;
        }

        #region Private helpers

        private void AddHeaders(ref HttpWebRequest request, string referer, string cookies, string xBrowserId = null)
        {
            request.Headers.Add("Origin", "https://portal1.c1.optymyze.com");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
            if (!string.IsNullOrWhiteSpace(referer))
            {
                request.Referer = referer;
            }
            if (!string.IsNullOrWhiteSpace(cookies))
            {
                request.Headers.Add(HttpRequestHeader.Cookie, cookies);
            }
            if (!string.IsNullOrWhiteSpace(xBrowserId))
            {
                request.Headers.Add("X-Browser-ID", xBrowserId);
            }
        }

        #endregion
    }
}

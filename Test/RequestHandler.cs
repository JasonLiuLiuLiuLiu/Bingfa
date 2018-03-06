using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Test
{
    enum RequestParam
    {
        AllowTimeout,
        LastRequest
    }
    class RequestHandler
    {
        /// <summary>
        /// Http Get 同步方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            HttpClient httpClient = new HttpClient();

            var t = httpClient.GetByteArrayAsync(url);
            t.Wait();
            var ret = Encoding.UTF8.GetString(t.Result);
            return ret;
        }
        public static string HttpPost(string url, string data = "", int timeout = 30000, RequestParam reqParam = RequestParam.AllowTimeout)
        {
            WebRequest req = CreateRequester(url, "POST", timeout);
            byte[] bs = Encoding.UTF8.GetBytes(data);
            req.ContentLength = bs.Length;
            Stream st = req.GetRequestStream();
            st.Write(bs, 0, bs.Length);
            st.Close();
            try
            {
                WebResponse res = req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream());
                return sr.ReadToEnd();
            }
            catch (WebException webEx)
            {
                // NLogManager.LogTrace("Post Error：" + webEx.Status.ToString());
                if (reqParam == RequestParam.AllowTimeout &&
                    (webEx.Status == WebExceptionStatus.Timeout ||
                    webEx.Status == WebExceptionStatus.ConnectFailure))
                {
                    // NLogManager.LogTrace("请求超时，重试请求：" + url);
                    return HttpPost(url, data, timeout, RequestParam.LastRequest);
                }
                return string.Empty;
            }
            catch (Exception e)
            {
                // NLogManager.LogError(e.GetType().FullName);
                // NLogManager.LogError(e);
                return string.Empty;
            }
        }
        /// <summary>
        /// 使用HttpClient 异步提交跑腿请求
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string HttpPostAsync(string uri, StringContent content)
        {
            HttpClient Client = new HttpClient();
            try
            {
                var result = Client.PostAsync(@uri, content).Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return result.Content.ReadAsStringAsync().Result;
                }
                return "请求出错";
            }
            catch
            {
                return null;
            }
        }

        private static WebRequest CreateRequester(string url, string method = "GET", int timeout = 30000)
        {
            //  NLogManager.LogTrace("CreateRequester " + url);
            WebRequest requester = WebRequest.Create(url);
            requester.Method = method;
            requester.Timeout = timeout;
            requester.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            return requester;
        }
    }
}

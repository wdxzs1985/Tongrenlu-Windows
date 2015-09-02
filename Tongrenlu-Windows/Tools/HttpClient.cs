﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using System.Collections;

using Newtonsoft.Json;
using log4net;

namespace Tongrenlu_Windows.Tools
{
    public class HttpClient
    {
        public const string UA = "Tongrenlu-Windows";
        public const int KB = 1024;
        public const int BUFFER_SIZE = KB * 4;

        public static readonly ILog log = LogManager.GetLogger(typeof(HttpClient));


        private CookieContainer cc = null;

        public HttpClient()
        {
            clearCookie();
        }

        public void clearCookie()
        {
            cc = new CookieContainer();
        }

        internal void addCookie(string url, string name, string value)
        {
            HttpWebRequest webreq = CreateRequest(url);
            var cookie = new Cookie(name, value);
            cc.Add(webreq.RequestUri, cookie);
        }

        public Cookie findCookie(string url, string name)
        {
            HttpWebRequest webreq = CreateRequest(url);
            var cookies = cc.GetCookies(webreq.RequestUri);

            foreach (Cookie cookie in cookies)
            {
                if(cookie.Name.Equals(name))
                {
                    return cookie;
                }
            }
            return null;
        }

        public string Get(string url)
        {
            var request = CreateRequest(url);

            HttpWebResponse response = null;
            string result = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                result = ResponseToString(response);

                if (log.IsDebugEnabled)
                {
                    log.Debug(result);
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
            return result;
        }

        public string Post(string url, Dictionary<string, string> values)
        {
            var request = CreateRequest(url);
            request = PostForm(request, values);

            HttpWebResponse response = null;
            string result = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                result = ResponseToString(response);

                if (log.IsDebugEnabled)
                {
                    log.Debug(result);
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
            return result;
        }

        public HttpWebRequest CreateRequest(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cc;
            request.UserAgent = UA;

            //request.Proxy = null;

            return request;
        }


        public HttpWebRequest PostForm(HttpWebRequest request, Dictionary<string, string> values)
        {
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            string param = "";
            foreach (string key in values.Keys)
            {
                param += String.Format("{0}={1}&", key, values[key]);
            }
            byte[] data = Encoding.UTF8.GetBytes(param);
            request.ContentLength = data.Length;

            Stream reqStream = request.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();

            return request;
        }

        public HttpWebRequest PostJson(HttpWebRequest request, Dictionary<string, object> values)
        {
            request.Method = "POST";
            request.ContentType = "application/json";

            string param = JsonConvert.SerializeObject(values);
            
            byte[] data = Encoding.UTF8.GetBytes(param);
            request.ContentLength = data.Length;

            Stream reqStream = request.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();

            return request;
        }

        public string ResponseToString(HttpWebResponse response)
        {
            var reader = new StreamReader(response.GetResponseStream());
            var responseString = reader.ReadToEnd();

            reader.Close();

            if (log.IsDebugEnabled)
            {
                log.Debug(responseString);
            }
            return responseString;
        }

        public string GetXHR(string url)
        {
            lock (this)
            {
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("[ GET] {0}", url);
                }
                var request = this.CreateRequest(url);
                

                HttpWebResponse response = null;
                string result = null;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    result = ResponseToString(response);
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error(ex.Message);
                    }
                }
                finally
                {
                    if (response != null)
                    {
                        response.Close();
                    }
                }
                return result;
            }
        }

        public string PostXHR(string url, Dictionary<string, object> values)
        {
            lock (this)
            {
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("[POST] {0}", url);
                }

                var request = this.CreateRequest(url);

                PostJson(request, values);

                HttpWebResponse response = null;
                string result = null;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    result = ResponseToString(response);
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error(ex.Message);
                    }
                }
                finally
                {
                    if (response != null)
                    {
                        response.Close();
                    }
                }
                return result;
            }
        }
        
        public bool DownloadBinary(string url, string fileName)
        {
            var request = CreateRequest(url);

            HttpWebResponse response = null;
            Stream sw = null;
            FileStream fs = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                long total = response.ContentLength;
                long loaded = 0;

                sw = response.GetResponseStream();

                //ファイルに書き込むためのFileStreamを作成
                fs = new FileStream(fileName, FileMode.Create);

                //応答データをファイルに書き込む
                byte[] readData = new byte[BUFFER_SIZE];
                for (;;)
                {
                    //データを読み込む
                    int readSize = sw.Read(readData, 0, readData.Length);
                    if (readSize == 0)
                    {
                        //すべてのデータを読み込んだ時
                        break;
                    }
                    //読み込んだデータをファイルに書き込む
                    fs.Write(readData, 0, readSize);

                    loaded += readSize;

                    Console.WriteLine("{0} / {1} loaded.", loaded, total);
                }
            }
            catch
            {

                return false;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }

            return true;
        }
    }
}

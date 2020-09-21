using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MyMicroService.Infrastruct.HttpWeb
{
    /// <summary>
    /// http请求帮助实现
    /// </summary>
    public class HttpWebHelperImpl : IHttpWebHelper
    {
        /// <summary>
        /// HttpGet
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">编码</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="headerDictionary">请求头</param>
        /// <returns></returns>
        public string HttpGet(string url, Encoding encoding, string contentType = "application/json; charset=utf-8", Dictionary<string, string> headerDictionary = null)
        {
            string result = "";
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }
                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "GET";
                httpWebRequest.ContentType = contentType;

                if (headerDictionary != null)
                {
                    foreach (var header in headerDictionary)
                    {
                        httpWebRequest.Headers.Add(header.Key, header.Value);
                    }
                }


                //获取服务器返回
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                //获取HTTP返回数据
                StreamReader sr = new StreamReader(httpWebResponse.GetResponseStream(), encoding);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //关闭连接和流
                if (httpWebResponse != null)
                    httpWebResponse.Close();
                if (httpWebRequest != null)
                    httpWebRequest.Abort();
            }
            return result;
        }

        /// <summary>
        /// HttpPost
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="data">请求数据</param>
        /// <param name="encoding">编码</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="headerDictionary">请求头</param>
        /// <returns></returns>
        public string HttpPost(string url, string data, Encoding encoding, string contentType = "application/json; charset=utf-8", Dictionary<string, string> headerDictionary = null)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);
                //request.UserAgent = USER_AGENT;
                request.Method = "POST";
                request.Timeout = 20 * 1000;
                //设置POST的数据类型和长度
                request.ContentType = contentType;
                byte[] byteData = encoding.GetBytes(data);
                request.ContentLength = byteData.Length;

                if (headerDictionary != null)
                {
                    foreach (var header in headerDictionary)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }


                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(byteData, 0, byteData.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), encoding);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                System.Threading.Thread.ResetAbort();
                throw e;
            }
            catch (WebException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        /// <summary>
        /// 直接确认，否则打不开    
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }
    }
}

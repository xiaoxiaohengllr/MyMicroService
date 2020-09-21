using System.Collections.Generic;
using System.Text;

namespace MyMicroService.Infrastruct.HttpWeb
{
    /// <summary>
    /// http请求帮助接口
    /// </summary>
    public interface IHttpWebHelper
    {
        /// <summary>
        /// HttpGet
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">编码</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="headerDictionary">请求头</param>
        /// <returns></returns>
        string HttpGet(string url, Encoding encoding, string contentType = "application/json; charset=utf-8", Dictionary<string, string> headerDictionary = null);

        /// <summary>
        /// HttpPost
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="data">请求数据</param>
        /// <param name="encoding">编码</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="headerDictionary">请求头</param>
        /// <returns></returns>
        string HttpPost(string url, string data, Encoding encoding, string contentType = "application/json; charset=utf-8", Dictionary<string, string> headerDictionary = null);
    }
}

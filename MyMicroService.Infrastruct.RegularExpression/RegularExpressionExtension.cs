using System.Text.RegularExpressions;

namespace MyMicroService.Infrastruct.RegularExpression
{
    /// <summary>
    /// 正则表达式扩展
    /// </summary>
    public static class RegularExpressionExtension
    {
        /// <summary>
        /// 验证字符串是否为url格式
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsURL(this string url)
        {
            return Regex.Match(url, @"[a-zA-z]+://[^\s]*").Success;
        }
    }
}

using System.Text.Json;

namespace MyMicroService.Infrastruct.JsonSerialize
{
    /// <summary>
    /// json序列化扩展
    /// </summary>
    public static class JsonSerializeExtension
    {
        /// <summary>
        /// 实体转为json字符串
        /// </summary>
        /// <param name="entity">需要转换的实体</param>
        /// <returns></returns>
        public static string ToJson(this object entity)
        {
            return JsonSerializer.Serialize(entity);
        }

        /// <summary>
        /// json字符串转为实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="json">需要转化的json字符串</param>
        /// <returns></returns>
        public static TEntity ToObject<TEntity>(this string json)
        {
            return JsonSerializer.Deserialize<TEntity>(json);
        }
    }
}

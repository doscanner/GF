using StackExchange.Redis;
using System;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace GF.Infrastructure.Cache.Redis
{
    public static class StackExchangeRedisExtensions
    {
        public static T Get<T>(this IDatabase cache, string key)
        {
            return Deserialize<T>(cache.StringGet(key));
        }

        public static object Get(this IDatabase cache, string key)
        {
            return Deserialize<object>(cache.StringGet(key));
        }

        public static void Set(this IDatabase cache, string key, object value)
        {
            cache.StringSet(key, Serialize(value));
        }

        public static string Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            var value = js.Serialize(o);
            value = Regex.Replace(value, @"\\/Date\((\d+)\)\\/", match =>
            {
                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                dt = dt.ToLocalTime();
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            });
            return value;
        }

        public static T Deserialize<T>(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return default(T);
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<T>(value);
        }

        //public static byte[] Serialize(object o)
        //{
        //    if (o == null)
        //    {
        //        return null;
        //    }

        //    BinaryFormatter binaryFormatter = new BinaryFormatter();
        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        binaryFormatter.Serialize(memoryStream, o);
        //        byte[] objectDataAsStream = memoryStream.ToArray();
        //        return objectDataAsStream;
        //    }
        //}

        //public static T Deserialize<T>(byte[] stream)
        //{
        //    if (stream == null)
        //    {
        //        return default(T);
        //    }

        //    BinaryFormatter binaryFormatter = new BinaryFormatter();
        //    using (MemoryStream memoryStream = new MemoryStream(stream))
        //    {
        //        T result = (T)binaryFormatter.Deserialize(memoryStream);
        //        return result;
        //    }
        //}
    }
}
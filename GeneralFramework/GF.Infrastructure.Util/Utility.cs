using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace GF.Infrastructure.Util
{
    /// <summary>
    /// 常用方法
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// MD5加密字符串方法
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns>返回加密过后的字符串</returns>
        public static string MD5Encrypt(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.Default.GetBytes(str);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            string tmp = "";
            for (int i = 0; i < md5data.Length - 1; i++)
            {
                tmp += md5data[i].ToString("x").PadLeft(2, '0');
            }
            return tmp;
        }

        /// <summary>
        /// 获取Hash值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        /// <summary>
        /// 序列化为JSON串
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns>JSON</returns>
        public static string Serialize(object obj)
        {
            if (obj == null)
                return null;

            JavaScriptSerializer js = new JavaScriptSerializer();
            var value = js.Serialize(obj);
            value = Regex.Replace(value, @"\\/Date\((\d+)\)\\/", match =>
            {
                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                dt = dt.ToLocalTime();
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            });
            return value;
        }

        /// <summary>
        /// 将JSON串反序列化
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="json">json</param>
        /// <returns>T</returns>
        public static T Deserialize<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default(T);

            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<T>(json);
        }
    }
}

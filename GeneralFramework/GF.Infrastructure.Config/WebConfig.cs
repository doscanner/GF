using System;
using System.Configuration;

namespace GF.Infrastructure.Config
{
    /// <summary>
    /// 配置文件
    /// Web.onfig
    /// </summary>
    public class WebConfig
    {
        /// <summary>
        /// OAuth认证暴露出来的地址
        /// </summary>
        public static string TokenPath = ConfigurationManager.AppSettings["TokenPath"];
        /// <summary>
        /// OAuth认证过期时间,单位：分钟
        /// </summary>
        public static string AccessTokenExpireTimeSpan = ConfigurationManager.AppSettings["AccessTokenExpireTimeSpan"];
    }
}

using System;

namespace GF.Infrastructure.Config
{
    /// <summary>
    /// 常量
    /// </summary>
    public class Constant
    {
        /// <summary>
        /// OAuth认证所需参数
        /// </summary>
        public static class GrantTypes
        {
            public const string GrantType = "grant_type";
            public const string UserName = "username";
            public const string Password = "password";
            public const string AuthorizationCode = "authorization_code";
            public const string ClientCredentials = "client_credentials";
            public const string RefreshToken = "refresh_token";
            public const string AuthenticationType = "OAuth2";
        }
    }
}

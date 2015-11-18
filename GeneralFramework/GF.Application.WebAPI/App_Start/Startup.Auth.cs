using System;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using GF.Application.WebAPI.Provider;
using Microsoft.Owin.Cors;
using GF.Infrastructure.Config;

namespace GF.Application.WebAPI
{
    partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AuthenticationType = Constant.GrantTypes.AuthenticationType,
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString(WebConfig.TokenPath),
                AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(int.Parse(WebConfig.AccessTokenExpireTimeSpan)),
                Provider = new OAuth2AuthorizationServerProvider(),
                RefreshTokenProvider = new OAuth2RefreshTokenProvider()
            };
            //AuthenticationType ：认证类型
            //AllowInsecureHttp : 如果允许客户端的 return_uri 参数不是 HTTPS 地址， 则设置为 true
            //TokenEndpointPath : 客户端应用可以直接访问并得到访问令牌的地址， 必须以前倒斜杠 "/" 开始， 例如： /Token
            //AccessTokenExpireTimeSpan ：Token过期时间
            //Provider : 应用程序提供和 OAuth 认证中间件交互的 IOAuthAuthorizationServerProvider 实例， 通常可以使用默认的	
            //OAuthAuthorizationServerProvider ， 并设置委托函数即可                
            //RefreshTokenProvider ：刷新令牌， 如果这个属性没有设置， 则不能从 /Token 刷新令牌

            // 令牌生成
            app.UseOAuthAuthorizationServer(OAuthServerOptions);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            //跨域处理
            app.UseCors(CorsOptions.AllowAll);
        }
    }
}

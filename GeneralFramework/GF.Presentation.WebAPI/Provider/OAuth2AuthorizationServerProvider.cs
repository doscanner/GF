using System;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using GF.Infrastructure.Config;

namespace GF.Presentation.WebAPI.Provider
{
    /// <summary>
    /// 应用程序提供和 OAuth 认证中间件交互的 IOAuthAuthorizationServerProvider 实例
    /// </summary>
    public class OAuth2AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// 第一步：客户端认证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string grant_type = context.Parameters[Constant.GrantTypes.GrantType];

            if (grant_type == Constant.GrantTypes.Password)
            {
                string username = context.Parameters[Constant.GrantTypes.UserName];
                string password = context.Parameters[Constant.GrantTypes.Password];

                //TODO 调用登录逻辑
                bool loginFlag = true;
                if (loginFlag)
                {
                    //把当前用户存入上下文
                    context.OwinContext.Set<string>("loginuser", username);
                    bool flag = context.Validated();
                }
                else
                {
                    context.Rejected();
                    return;
                }
            }
            else if (grant_type == Constant.GrantTypes.RefreshToken)
            {
                bool flag = context.Validated();
            }
            else
            {
                context.Rejected();
                return;
            }
        }

        /// <summary>
        /// 第二步：授予资源所有者凭据
        /// grant_type:password
        /// 如：grant_type=password&username=admin&password=123456
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //获取用户信息、验证用户身份合法性
            string loginuser = context.OwinContext.Get<string>("loginuser");

            string clientId = context.UserName;
            string clientSecret = context.Password;


            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim("sub", context.UserName));
            oAuthIdentity.AddClaim(new Claim("role", "user"));

            // 创建元数据传递到刷新令牌提供程序
            var props = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        { "as:client_id", context.UserName }
                    });

            var ticket = new AuthenticationTicket(oAuthIdentity, props);
            bool flag = context.Validated(ticket);
        }
    }
}

/*
 *  OWIN是Open Web Server Interface for .NET的首字母缩写，他的定义如下：
 *      OWIN在.NET Web Servers与Web Application之间定义了一套标准接口，
 *      OWIN的目标是用于解耦Web Server和Web Application。基于此标准，
 *      鼓励开发者开发简单、灵活的模块，从而推进.NET Web Development开源生态系统的发展 
 *  
 *  OWIN规范的定义
 *      OWIN定义了Host、Server、Middleware、Application 4Layers（层）
 *      Application Delegate（应用程序委托）
 *      Environment Dictionary（环境字典）
 *  
 *  使用 OWIN 搭建 OAuth2 认证服务器
 *      认证服务器指 authorization server ， 负责在资源所有者 （最终用户） 通过认证之后， 
 *      向客户端应用颁发凭据 (code) 和对客户端授权 (access_token) 。
 * **/
using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(GF.Application.WebAPI.Startup))]

namespace GF.Application.WebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 有关如何配置应用程序的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=316888

            ConfigureAuth(app);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using GF.Infrastructure.Config;
using GF.Infrastructure.Entity;
using GF.Infrastructure.Util;
using System.Net.Http;

namespace GF.Application.WebAPI.Filter
{
    /// <summary>
    /// 身份、权限验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeFilter : AuthorizeAttribute
    {
        /// <summary>
        /// 返回值为false时执行
        /// </summary>
        /// <param name="actionContext"></param>
        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
            if (actionContext.Response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var response = Utility.Serialize(new Result() { Code = ResultCode.Unauthorized, Msg = TipResource.Unauthorized });
                actionContext.Response = new HttpResponseMessage { Content = new StringContent(response, Encoding.UTF8, "application/json") };
            }
        }

        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //允许匿名访问
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Count > 0)
                return true;

            //验证是否认证通过
            ClaimsPrincipal principal = Thread.CurrentPrincipal as ClaimsPrincipal;
            if (!principal.Identity.IsAuthenticated)
                return false;

            //当前用户ID
            int userId = 0;
            int.TryParse(principal.Identity.Name, out userId);
            if (userId <= 0)
                return false;

            #region 当前登录用户状态

            //var user = RedisHelp.GetLoginUserCacheNotNull(userId);
            //if (user == null)
            //    return false;
            //if (user.AccessToken == null || string.IsNullOrEmpty(user.AccessToken.access_token))
            //    throw new AuthorizeException("未认证用户,无法访问");

            //if ((int)user.Type != 0)
            //{
            //    if (user.Status != UserStatus.Normal)
            //        throw new UseStatusException("用户状态异常,请联系相关人员");
            //    if (user.Branch != null && !user.Branch.Status)
            //        throw new UseStatusException("用户所属分社状态异常,请联系相关人员");
            //    if (user.Special != null && user.Special.Status != SpecialStatus.Normal)
            //        throw new UseStatusException("用户所属专线状态异常,请联系相关人员");
            //}
            #endregion

            #region 单点登录判断
            //if (System.Configuration.ConfigurationManager.AppSettings["SingleSignOn"] == "1")
            //{
            //    if (actionContext.Request.Headers.Authorization != null && actionContext.Request.Headers.Authorization.Scheme == "Bearer")
            //    {
            //        if (user.AccessToken.access_token != actionContext.Request.Headers.Authorization.Parameter)
            //            throw new AccountLoginInOtherPlacesException("用户已在其他地方登录,请重新登录");
            //    }
            //}
            #endregion

            #region 超过5小时未登录 需重新登录
            //TimeSpan lastTime = DateTime.Now - user.LastVistTime;
            //if (lastTime.TotalMinutes > int.Parse(System.Configuration.ConfigurationManager.AppSettings["LoginLastVistTime"]))
            //{
            //    throw new AuthorizeException("登录身份已失效,请重新登录");
            //}
            #endregion

            //请求URL
            string url = actionContext.Request.RequestUri.AbsolutePath.ToLower();

            //CheckAuth(url, user);

            //user.LastVistTime = DateTime.Now;
            //RedisHelp.RefreshLoginUserCache(user);

            return base.IsAuthorized(actionContext);
        }

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
        }

        private void CheckAuth(string requestUrl)
        {

        }
    }
}

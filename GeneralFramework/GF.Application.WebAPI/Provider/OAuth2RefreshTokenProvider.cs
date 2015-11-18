using System;
using Microsoft.Owin.Security.Infrastructure;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Owin.Security;

namespace GF.Application.WebAPI.Provider
{
    /// <summary>
    /// 刷新令牌
    /// </summary>
    public class OAuth2RefreshTokenProvider : IAuthenticationTokenProvider
    {
        //用ConcurrentDictionary是为了线程安全
        private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var guid = Guid.NewGuid().ToString("n");

            _refreshTokens.TryAdd(guid, context.Ticket);

            context.SetToken(guid);
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            AuthenticationTicket ticket;
            if (_refreshTokens.TryRemove(context.Token, out ticket))
            {
                context.SetTicket(ticket);
            }
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}

using Microsoft.AspNetCore.Http;
using System.Net;

namespace Portfolio.Infrastructure.Security
{
    public class TokenManagerMiddleware : IMiddleware
    {
        private readonly TokenManager _tokenManager;

        public TokenManagerMiddleware(TokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (await _tokenManager.IsCurrentActiveToken())
            {
                await next(context);

                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Portfolio.Infrastructure.Helpers;
using System.Security.Claims;

namespace Portfolio.Infrastructure.Security
{
    public class TokenManager
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _context;
        private readonly AppSettings _appSettings;

        public Guid CodigoUsuarioLogado => Guid.Parse(_context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

        public TokenManager(IHttpContextAccessor context, IOptions<AppSettings> appSettings, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
            _appSettings = appSettings.Value;
        }

        public async Task<bool> IsCurrentActiveToken()
        {
            return await IsActiveAsync(GetCurrentAsync());
        }

        public async Task DeactivateCurrentAsync()
        {
            await DeactivateAsync(GetCurrentAsync());
        }

        public async Task<bool> IsActiveAsync(string token)
        {
            return await _cache.GetStringAsync(GetKey(token)) == null;
        }

        public async Task DeactivateAsync(string token)
        {
            await _cache.SetStringAsync(GetKey(token),
                " ", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(_appSettings.ExpiryMinutes)
                });
        }

        private string GetCurrentAsync()
        {
            var authorizationHeader = _context
                .HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
        }

        private static string GetKey(string token)
        {
            return $"tokens:{token}:deactivated";
        }
    }
}

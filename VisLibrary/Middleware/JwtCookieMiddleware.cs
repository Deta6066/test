// Middleware/JwtCookieMiddleware.cs
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VisLibrary.Middleware
{
    public class JwtCookieMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtCookieMiddleware> _logger;

        public JwtCookieMiddleware(RequestDelegate next, ILogger<JwtCookieMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 檢查是否存在 Authorization 標頭
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                // 從 Cookie 中讀取 JWT Token
                var token = context.Request.Cookies["jwtToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    _logger.LogInformation("JwtCookieMiddleware: Adding Authorization header.");
                    context.Request.Headers.Add("Authorization", $"Bearer {token}");
                }
                else
                {
                    _logger.LogWarning("JwtCookieMiddleware: jwtToken cookie is missing.");
                }
            }

            await _next(context);
        }
    }
}

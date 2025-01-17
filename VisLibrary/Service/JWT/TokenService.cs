using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Service.JWT
{
    public class TokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetTokenCookies(string accessToken, string refreshToken)
        {
            var context = _httpContextAccessor.HttpContext;

            if (context != null)
            {
                context.Response.Cookies.Append("jwtToken", accessToken, new CookieOptions
                {
                    HttpOnly = true,
                    //Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(15)
                });

                context.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    //Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                });
            }
        }

        public void ClearTokenCookies()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context != null)
            {
                context.Response.Cookies.Delete("jwtToken");
                context.Response.Cookies.Delete("refreshToken");
            }
        }
        /// <summary>
        /// 取得 Refresh Token 從 Cookie
        /// </summary>
        /// <returns>Refresh Token 字串或 null</returns>
        public string? GetRefreshToken()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return null;

            context.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
            return refreshToken;
        }
        /// <summary>
        /// 從 Cookie 中取得 Access Token
        /// </summary>
        public string? GetAccessToken()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return null;

            context.Request.Cookies.TryGetValue("jwtToken", out var accessToken);
            return accessToken;
        }
    }

}

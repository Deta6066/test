using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VisLibrary.Service.Interface;
using VisLibrary.Service.JWT;

namespace VisLibrary.Middleware
{
    public class SingleIPMiddleware
    {
        private readonly RequestDelegate _next;


        public SingleIPMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context,
                                   IJwtTokenService jwtTokenService,
                                   ISessionService sessionService,
                                   TokenService tokenService)
        {
            var token = context.Request.Cookies["jwtToken"]; // or Authorization header
            var path = context.Request.Path.Value.ToLower();

            if (!string.IsNullOrEmpty(token)&&!path.Contains("/login"))
            {
                var principal = jwtTokenService.ValidateToken(token);
                if (principal != null)
                {
                    //要的是帳號 但JWT存的是名字 永遠不會被踢出 要修改
                    var account = principal.FindFirst(ClaimTypes.Name)?.Value;

                    if (!string.IsNullOrEmpty(account))
                    {
                        var storedIP = await sessionService.GetIPByAccountAsync(account);
                        var currentIP = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                        if (!string.IsNullOrEmpty(storedIP) && storedIP != currentIP)
                        {
                            // IP_CHANGED => 清除舊 Token
                            tokenService.ClearTokenCookies();

                            // 表示該帳號已在另一個IP登入
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            var errorObj = new
                            {
                                code = "IP_CHANGED",
                                message = "帳號已在其他地方登入，您被踢出。"
                            };

                            await context.Response.WriteAsync(JsonSerializer.Serialize(errorObj));
                            return;
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}

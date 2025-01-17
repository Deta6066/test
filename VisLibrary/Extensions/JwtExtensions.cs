using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Logging;

namespace VisLibrary.Extensions
{
    /// <summary>
    /// 提供 JWT 驗證配置的擴展方法。
    /// </summary>
    public static class JwtExtensions
    {
        /// <summary>
        /// 添加 JWT 驗證配置。
        /// </summary>
        /// <param name="services">服務集合。</param>
        /// <param name="configuration">配置對象。</param>
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var loggerFactory = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
                        var logger = loggerFactory.CreateLogger("JwtExtensions");
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                            var expiredException = context.Exception as SecurityTokenExpiredException;
                            logger.LogWarning("Access Token has expired at {ExpiredTime}. Current time: {CurrentTime}",
                                expiredException.Expires,
                                DateTime.UtcNow);
                        }
                        else
                        {
                            logger.LogError(context.Exception, "Authentication failed.");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true, // 驗證過期時間
                    ClockSkew = TimeSpan.Zero // 默認的時間偏移為 5 分鐘，設置為 0 避免偏移
                };
            });
        }
    }
}

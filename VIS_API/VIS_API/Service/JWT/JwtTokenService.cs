using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Middleware;
using VIS_API.Service.Interface;

namespace VIS_API.Service.JWT
{
    /// <summary>
    /// 用於生成 JWT 的服務類別。
    /// </summary>
    public class JwtTokenService: IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<JwtTokenService> _logger;

        /// <summary>
        /// 初始化 JwtTokenService 類的新實例。
        /// </summary>
        /// <param name="key">用於簽名的密鑰。</param>
        /// <param name="issuer">JWT 的發行者。</param>
        /// <param name="audience">JWT 的受眾。</param>
        public JwtTokenService(IOptions<JwtSettings> jwtSettingsOptions, ILogger<JwtTokenService> logger)
        {
            _jwtSettings = jwtSettingsOptions.Value;
            _logger = logger;
        }

        /// <summary>
        /// 生成 JWT 令牌。
        /// </summary>
        /// <param name="acc">帳號。</param>
        /// <returns>JWT 令牌。</returns>
        public string GenerateAccessToken(string acc,int userId, int companyFk)
        {
            // 創建 JwtSecurityTokenHandler 實例
            var tokenHandler = new JwtSecurityTokenHandler();

            // 將密鑰編碼為字節數組
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            // 獲取當前的 UTC 時間
            var now = DateTime.Now;

            // 設置 SecurityTokenDescriptor，包括聲明、過期時間、發行者、受眾和簽名憑證
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // 設置 JWT 的聲明
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, acc), // 用戶名聲明
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()), // 使用者ID
                new Claim("companyFk", companyFk.ToString()), // 公司外鍵
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // 唯一標識符聲明
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64) // 發行時間聲明
            }),
                // 設置 JWT 的過期時間為當前時間後 60 分鐘
                Expires = now.AddMinutes(3),
                // 設置發行者
                Issuer = _jwtSettings.Issuer,
                // 設置受眾
                Audience = _jwtSettings.Audience,
                // 設置簽名憑證，使用對稱密鑰和 HMAC SHA256 算法
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // 創建 JWT
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 返回 JWT 字符串
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// 生成 Refresh Token。
        /// </summary>
        /// <returns>Refresh Token 字串。</returns>
        public string GenerateRefreshToken()
        {
            // 使用隨機數據生成 Refresh Token
            var randomNumber = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        /// <summary>
        /// 驗證 Token，若合法則回傳 ClaimsPrincipal，否則回傳 null。
        /// </summary>
        /// <param name="token">要驗證的 JWT。</param>
        /// <returns>ClaimsPrincipal 或 null</returns>
        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero // 時鐘偏移設為0，避免測試時混淆
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                // 也可再驗證 validatedToken 類型或其他自定義檢查
                return principal;
            }
            catch (SecurityTokenException ex)
            {
                // 包含 Token 過期、簽名錯誤等
                _logger.Log(LogLevel.Information, ex,"JWT Token過期");
                return null;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Information, ex, "JWT Token過期");
                return null;
            }
        }
    }
}

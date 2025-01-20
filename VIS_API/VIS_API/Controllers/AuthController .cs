using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VIS_API.Models;
using VIS_API.Repositories.Interface;
using VIS_API.Service;
using VIS_API.Service.Interface;
using VIS_API.Service.JWT;
using VIS_API.UnitWork;

namespace VIS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ISUser _sUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly TokenService _tokenService;
        private readonly ISessionService _sessionService;

        public AuthController(IConfiguration configuration, ISUser sUser,IUnitOfWork unitOfWork, IRefreshTokenRepository refreshTokenRepository, TokenService tokenService, ISessionService sessionService, IJwtTokenService jwtTokenService)
        {
            _configuration = configuration;
            var key = configuration["Jwt:Key"];
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            _jwtTokenService =  jwtTokenService;
            _sUser = sUser;
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _sessionService = sessionService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin model)
        {
            try
            {
                await _unitOfWork.OpenAsyncConnection();
                MUser? result = await _sUser.GetByAcc(model.Username, model.Password);
                if (result == null)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new
                    {
                        code = "LOGIN_FAILED",
                        message = "帳號密碼錯誤"
                    });
                }
                var token = _jwtTokenService.GenerateAccessToken(result.acc, result.pk, result.company_fk);
                var refreshToken = _jwtTokenService.GenerateRefreshToken();
                var refreshTokenEntity = new RefreshToken
                {
                    Token = refreshToken,
                    Account = result.acc,
                    ExpiryDate = DateTime.Now.AddDays(7),
                    IsUsed = false,
                    IsRevoked = false,
                    CompanyFk = result.company_fk
                };
                await _refreshTokenRepository.AddAsync(refreshTokenEntity);

                // 使用 TokenService 設置 Cookies
                _tokenService.SetTokenCookies(token, refreshToken);

                // 5. SessionService 設置 (account -> ip)
                var currentIP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                await _sessionService.SetIPForAccountAsync(model.Username, currentIP);

                return Ok(new { Message = "Login successful" });
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            // 從 Cookie 中取得 Access Token 和 Refresh Token
            var accessToken = _tokenService.GetAccessToken();
            var refreshToken = _tokenService.GetRefreshToken();

            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new { Message = "Refresh Token is required." });
            }

            var existingRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (existingRefreshToken == null || existingRefreshToken.IsUsed || existingRefreshToken.IsRevoked || existingRefreshToken.ExpiryDate < DateTime.Now)
            {
                return Unauthorized(new { Message = "Invalid refresh token" });
            }

            bool isAccessTokenExpired = true;

            if (string.IsNullOrEmpty(accessToken))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    code = "Token_Missing",
                    message = "認證失效"
                });
            }
            else
            {
                isAccessTokenExpired = IsTokenExpired(accessToken);

            }

            if (!isAccessTokenExpired)
            {
                // Access Token 仍然有效，不需要刷新
                return Ok(new { Message = "Access token is still valid." });
            }

            // Access Token 已過期，需要刷新

            // 標記 Refresh Token 為已使用
            existingRefreshToken.IsUsed = true;
            await _refreshTokenRepository.UpdateAsync(existingRefreshToken);

            // 生成新的 Access Token 和 Refresh Token
            var newAccessToken = _jwtTokenService.GenerateAccessToken(existingRefreshToken.Account, existingRefreshToken.Id, existingRefreshToken.CompanyFk);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            // 儲存新的 Refresh Token
            var newRefreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                Account = existingRefreshToken.Account,
                ExpiryDate = DateTime.Now.AddDays(7),
                IsUsed = false,
                IsRevoked = false,
                CompanyFk = existingRefreshToken.CompanyFk
            };

            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity);

            // 使用 TokenService 設置新的 Cookies
            _tokenService.SetTokenCookies(newAccessToken, newRefreshToken);

            // 4. 解析 newAccessToken，取出 exp
            long newExpUnix = 0;
            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.CanReadToken(newAccessToken))
            {
                var jwtToken = tokenHandler.ReadJwtToken(newAccessToken);
                var expClaim = jwtToken.Claims
                    .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
                if (!string.IsNullOrEmpty(expClaim))
                {
                    newExpUnix = long.Parse(expClaim);
                }
            }
            return Ok(new { Message = "Refresh successful", newExpiry = newExpUnix  });
            }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // 從 Cookie 中取得 Refresh Token
            var refreshToken = _tokenService.GetRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new { Message = "Refresh Token is required." });
            }

            var existingRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (existingRefreshToken == null)
            {
                return BadRequest(new { Message = "Invalid refresh token" });
            }

            // 標記 Refresh Token 為已撤銷
            existingRefreshToken.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(existingRefreshToken);

            // 清除 Cookies
            _tokenService.ClearTokenCookies();
            await _sessionService.DeleteSessionAsync(existingRefreshToken.Account);

            return Ok(new { Message = "Logged out successfully" });
        }
        private static bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(token))
            {
                // Token 無法被解析，視為無效或過期
                return true;
            }

            var jwtToken = tokenHandler.ReadJwtToken(token);

            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;

            if (expClaim == null)
            {
                // 沒有找到過期聲明，視為無效或過期
                return true;
            }

            // 將 exp 聲明轉換為 DateTime
            var exp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim)).DateTime;

            // 比較過期時間與當前 UTC 時間 
            return exp < DateTime.Now;
        }
        [HttpGet("me")]
        public IActionResult Me()
        {
            // 從 Cookie 中取出 Token -> Validate
            var token = Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(token))
                return Unauthorized();

            var principal = _jwtTokenService.ValidateToken(token);
            if (principal == null)
                return Unauthorized();

            // 解析 exp
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
            if (expClaim == null)
                return Unauthorized();

            // 回傳 exp
            long expUnix = long.Parse(expClaim);
            return Ok(new { exp = expUnix, userId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Service.Interface
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(string username, int userId, int companyFk);
        string GenerateRefreshToken();
        ClaimsPrincipal? ValidateToken(string token);
    }
}

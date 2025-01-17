using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VisLibrary.Models;
using VisLibrary.Models.CNC;
using VisLibrary.Repositories.Interface;
using VisLibrary.Service.CNC;
using VisLibrary.Service.Interface;
using VisLibrary.Service.JWT;

namespace VIS_API.Controllers.CNC
{
    /// <summary>
    /// 異常歷程統計控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AnomalyHistoryController : Controller
    {
        ISnomalyHistory _sNomalyHistory;
        IMapper _mapper;
        TokenService _tokenService;
        IRefreshTokenRepository _refreshTokenRepository;
        public AnomalyHistoryController(ISnomalyHistory sNomalyHistory,IMapper mapper,TokenService tokenService,IRefreshTokenRepository refreshTokenRepository)
        {
            _sNomalyHistory = sNomalyHistory;
            _mapper = mapper;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        /// <summary>
        /// 取得異常歷程統計
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPost("GetAnomalyHistory")]
        public async Task<ApiResponse<VMAnomalyHistory>> GetAnomalyHistory([FromBody] MAnomalyHistoryParameter parameter)
        {
            VMAnomalyHistory vAnomalyHistory = new VMAnomalyHistory();
            List<MAnomalyHistoryDetail> mAnomalyHistories = new List<MAnomalyHistoryDetail>();
            ApiResponse<VMAnomalyHistory> response = new ApiResponse<VMAnomalyHistory>(vAnomalyHistory);
            try
            {
                var refreshToken = _tokenService.GetRefreshToken();
               var existingRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
                if (existingRefreshToken == null || existingRefreshToken.IsUsed || existingRefreshToken.IsRevoked || existingRefreshToken.ExpiryDate < DateTime.UtcNow)
                {
                    return new ApiResponse<VMAnomalyHistory>(new VMAnomalyHistory(), false, ResponseStatusCode.Unauthorized, "認證過期");
                }
                int companyID = existingRefreshToken.CompanyFk;
                mAnomalyHistories = await _sNomalyHistory.GetAnomalyHistory(parameter,companyID);
                vAnomalyHistory.MAnomalyHistories= _mapper.Map<List<VMAnomalyHistoryDetail>>(mAnomalyHistories);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
          
            return response;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using VIS_API.Models;
using VIS_API.Models.CNC;
using VIS_API.Models.View;
using VIS_API.Service.Interface;

namespace VIS_API.Controllers.CNC
{
    /// <summary>
    /// 稼動比例分析控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OperationalRateController : Controller
    {
        ISOperationalRate _sOperationalRate;
        public OperationalRateController(ISOperationalRate sOperationalRate)
        {
            _sOperationalRate = sOperationalRate;
        }
        /// <summary>
        /// 取得稼動率統計資料
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetOperationalRate")]
        public async Task<ApiResponse<VMOperationalRate>> GetVMOperationalRate(MOperationalRateParamater filter)
        {
            VMOperationalRate vmOperationalRate = new VMOperationalRate();
            vmOperationalRate.mOperationalRateList = await _sOperationalRate.GetOperationalRate(filter);
            if(vmOperationalRate.mOperationalRateList == null)
            {
                return new ApiResponse<VMOperationalRate>(new VMOperationalRate(), success: false, message: "null");
            }

            return new ApiResponse<VMOperationalRate>(vmOperationalRate, success: true);
            //return vmOperationalRate;
        }
    }
}

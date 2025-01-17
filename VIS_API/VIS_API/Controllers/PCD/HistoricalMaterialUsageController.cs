using Microsoft.AspNetCore.Mvc;
using VisLibrary.Business;
using VisLibrary.Models;
using VisLibrary.Models.PCD;
using VisLibrary.Models.View;
using VisLibrary.Service.Interface;
using VisLibrary.Utilities;

namespace VIS_API.Controllers.PCD
{
    /// <summary>
    /// 歷史用量API
    /// </summary>
    [ApiController]
    public class HistoricalMaterialUsageController : Controller
    {
        ISHistoricalMaterialUsage _SHistoricalMaterialUsage;
        public HistoricalMaterialUsageController(ISHistoricalMaterialUsage sHistorical)
        {
            _SHistoricalMaterialUsage = sHistorical;
        }
        /// <summary>
        /// 取得歷史用料統計表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("[Controller]/GetHistoricalMaterials")]
        public async Task<ApiResponse<List<HistoricalMaterialUsageModel>>> GetHistoricalMaterials(
            [FromBody] MHistoricalMaterialUsageParameter filter, [FromHeader(Name = "CompanyID")] int CompanyID = 0)
        {
            List<HistoricalMaterialUsageModel> list = new List<HistoricalMaterialUsageModel>();
            try
            {
                list = await _SHistoricalMaterialUsage.GetHistoricalMaterials(filter, CompanyID);
                if (list == null)
                {
                    return new ApiResponse<List<HistoricalMaterialUsageModel>>(new List<HistoricalMaterialUsageModel>(), success: false, ResponseStatusCode.NoContent, "null");
                }
                return new ApiResponse<List<HistoricalMaterialUsageModel>>(list, success: true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<HistoricalMaterialUsageModel>>(new List<HistoricalMaterialUsageModel>(), success: false, ResponseStatusCode.NotFound, ex.Message);

            }
           
        }
        /// <summary>
        /// 取得領料紀錄
        /// </summary>
        /// <param name="filter">領料紀錄篩選器</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[Controller]/GetHistoricalMaterialUsageModel_Detail")]
        public async Task<ApiResponse<VMHistoricalMaterialUsage>> GetHistoricalMaterialUsageModel_Detail(
                   [FromBody] MHistoricalMaterialUsageParameter filter)
        {
            VMHistoricalMaterialUsage vMHistoricalMaterialUsage = new VMHistoricalMaterialUsage();
            List<HistoricalMaterialUsageModel_Detail> list = new List<HistoricalMaterialUsageModel_Detail>();
            try
            {
                list = await _SHistoricalMaterialUsage.GetHistoricalMaterialUsageModel_Detail(filter);
                vMHistoricalMaterialUsage.HistoricalMaterialUsageModelList = list;
                if(list == null)
                {
                    return new ApiResponse<VMHistoricalMaterialUsage>(new VMHistoricalMaterialUsage(), success: false,ResponseStatusCode.NoContent,"null");
                }
                return new ApiResponse<VMHistoricalMaterialUsage>(vMHistoricalMaterialUsage, success: true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<VMHistoricalMaterialUsage>(new VMHistoricalMaterialUsage(), success: false, ResponseStatusCode.NotFound, ex.Message);

            }
        }
    }
}

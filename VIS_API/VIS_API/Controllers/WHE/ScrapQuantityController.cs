using Microsoft.AspNetCore.Mvc;
using VIS_API.Models;
using VIS_API.Models.WHE;
using VIS_API.Service.Interface;

namespace VIS_API.Controllers.WHE
{
    /// <summary>
    /// 報廢數量統計控制器
    /// </summary>
    [ApiController]
    public class ScrapQuantityController : Controller
    {
        IScrapQuantity _scrapQuantityService;
        public ScrapQuantityController(IScrapQuantity scrapQuantityService)
        {
            _scrapQuantityService = scrapQuantityService;
        }
        /// <summary>
        /// 取得報廢數量統計表
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[Controller]/GetScrapQuantity")]
        public async Task<ApiResponse<VMScrapQuantity>> GetScrapQuantity([FromBody] MScrapQuantityParameter filter)
        {
            //透過篩選器取得報廢數量統計
            List<MScrapQuantity> scrapQuantityModels = new List<MScrapQuantity>();
            try
            {
                scrapQuantityModels = await _scrapQuantityService.GetScrapQuantityList(filter.CompanyID, filter);
                if (scrapQuantityModels == null)
                {
                    return new ApiResponse<VMScrapQuantity>(new VMScrapQuantity(), success: false, ResponseStatusCode.NoContent, "null");
                }
                VMScrapQuantity scrapQuantity = new VMScrapQuantity();
                scrapQuantity.MScrapQuantityList = scrapQuantityModels;
                return new ApiResponse<VMScrapQuantity>(scrapQuantity, success: true);
                //  return scrapQuantity;
            }
            catch (Exception ex)
            {
                return new ApiResponse<VMScrapQuantity>(new VMScrapQuantity(), success: false, ResponseStatusCode.NotFound, ex.Message);
            }
          
        }
      
        /// <summary>
        /// 刪除報廢數量
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="scrapQuantity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[Controller]/DeleteScrapQuantity")]
        public async Task<int> DeleteScrapQuantity([FromHeader(Name = "companyID")] int companyID, [FromBody] MScrapQuantity scrapQuantity)
        {
            //刪除報廢數量
            int result = await _scrapQuantityService.DeleteScrapQuantity(companyID, scrapQuantity.ID);
            return result;
        }
        /// <summary>
        /// 更新報廢數量
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="scrapQuantity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[Controller]/UpdateScrapQuantity")]
        public async Task<int> UpdateScrapQuantity([FromHeader(Name = "companyID")] int companyID, [FromBody] MScrapQuantity scrapQuantity)
        {
            //更新報廢數量
            int result = await _scrapQuantityService.UpdateScrapQuantity(companyID, scrapQuantity);
            return result;
        }
    }
}

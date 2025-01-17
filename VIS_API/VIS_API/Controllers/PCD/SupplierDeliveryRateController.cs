using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using VisLibrary.Models;
using VisLibrary.Models.View;
using VisLibrary.Service;

namespace VIS_API.Controllers.PCD
{

    /// <summary>
    /// 供應商達交率控制器
    /// </summary>
    [ApiController]
    public class SupplierDeliveryRateController : Controller
    {
        ISSupplierDeliveryRate _supplierDeliveryRateService;
        public SupplierDeliveryRateController(ISSupplierDeliveryRate sSupplierDeliveryRate)
        {
            _supplierDeliveryRateService = sSupplierDeliveryRate;
        }
        /// <summary>
        /// 取得供應商達交率明細表
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[Controller]/GetSupplierDeliveryRateDetail")]
        public async Task<ApiResponse<VMSupplierScoreDetail>> GetSupplierDeliveryRateDetail([FromBody] MSupplierScoreParameter filter)
        {
            // SupplierDeliveryRateBusiness supplierDeliveryRateBusiness = new SupplierDeliveryRateBusiness();
            //透過篩選器取得供應商達交率明細表
            List<MSupplierScoreDetail> supplierScoreDetailModels = new List<MSupplierScoreDetail>();
            try
            {
                supplierScoreDetailModels = await _supplierDeliveryRateService.GetSupplierDeliveryRateDetail(filter.company_fk, filter);
                VMSupplierScoreDetail vMSupplierScoreDetail = new VMSupplierScoreDetail();
                if (supplierScoreDetailModels == null)
                {
                    return new ApiResponse<VMSupplierScoreDetail>(new VMSupplierScoreDetail(), success: false, ResponseStatusCode.NoContent, "null");
                }
                vMSupplierScoreDetail.SupplierScoreDetailList = supplierScoreDetailModels;
                return new ApiResponse<VMSupplierScoreDetail>(vMSupplierScoreDetail, success: true);
                // return vMSupplierScoreDetail;
            }
            catch (Exception ex)
            {
                return new ApiResponse<VMSupplierScoreDetail>(new VMSupplierScoreDetail(), success: false, ResponseStatusCode.NotFound, ex.Message);
            }
           
        }
        /// <summary>
        /// 取得供應商達交率統計表
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[Controller]/GetSupplierDeliveryRate")]
        public async Task<ApiResponse<List<MSupplierScore>>> GetSupplierDeliveryRate([FromBody] MSupplierScoreParameter filter, [FromHeader] int CompanyID = 0)
        {
            //透過篩選器取得供應商達交率統計表
            List<MSupplierScore> supplierScoreModels = new List<MSupplierScore>();
            try
            {
                supplierScoreModels = await _supplierDeliveryRateService.GetSupplierDeliveryRate(CompanyID, filter);
                if (supplierScoreModels == null)
                {
                    return new ApiResponse<List<MSupplierScore>>(new List<MSupplierScore>(), success: false, ResponseStatusCode.NoContent, "null");
                }
                return new ApiResponse<List<MSupplierScore>>(supplierScoreModels, success: true);
            }
            catch (Exception)
            {
                return new ApiResponse<List<MSupplierScore>>(new List<MSupplierScore>(), success: false, ResponseStatusCode.NotFound, "null");
            }
          
        }
    }
}

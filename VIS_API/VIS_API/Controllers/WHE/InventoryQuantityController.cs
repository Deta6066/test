using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VIS_API.Models;
using VIS_API.Models.View;
using VIS_API.Models.WHE;
using VIS_API.Service.Interface;

namespace VIS_API.Controllers.WHE
{
    /// <summary>
    /// 庫存數量控制器
    /// </summary>
    [ApiController]
    public class InventoryQuantityController : Controller
    {
        ISInventory _inventoryBusiness;
        public InventoryQuantityController(ISInventory inventoryBusiness)
        {
            _inventoryBusiness = inventoryBusiness;
        }
        /// <summary>
        /// 取得庫存明細
        /// </summary>
        /// <param name="companyID">公司雲端ID</param>
        /// <param name="filter">庫存篩選器</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[Controller]/GetInventoryDetail")]
        //[Authorize]
        //[FromHeader(Name = "companyID")] int companyID,
        public async Task<ApiResponse<VMInventoryDetail>> GetInventoryDetail( [FromBody] MInventoryParameter filter)
        {
            List<MInventoryDetail>? result = new List<MInventoryDetail>();
            //透過Service取得庫存明細
            // result = service.GetInventoryDetail(companyID, filter);
            try
            {
                result = await _inventoryBusiness.GetInventoryDetail(filter, filter.CompanyID);
                if(result == null)
                {
                    return new ApiResponse<VMInventoryDetail>(new VMInventoryDetail(), success: false, ResponseStatusCode.NoContent, "null");
                }
                VMInventoryDetail vmInventoryDetail = new VMInventoryDetail();
                vmInventoryDetail.InventoryDetailList = result;
                return new ApiResponse<VMInventoryDetail>(vmInventoryDetail, success: true);
                // return vmInventoryDetail;
            }
            catch (Exception ex)
            {
                return new ApiResponse<VMInventoryDetail>(new VMInventoryDetail(), success: false, ResponseStatusCode.NotFound, ex.Message);
            }
          
        }
    }
}

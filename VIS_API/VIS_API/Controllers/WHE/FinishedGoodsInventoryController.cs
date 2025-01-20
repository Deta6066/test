using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VIS_API.Business;
using VIS_API.Models;
using VIS_API.Models.View;
using VIS_API.Models.WHE;
using VIS_API.Service.Interface;

namespace VIS_API.Controllers.WHE
{
    /// <summary>
    /// 成品庫存控制器
    /// </summary>
    [ApiController]
    public class FinishedGoodsInventoryController : Controller
    {
        ISFinishedGoodsInventory _sFinishedGoodsInventory;
        public FinishedGoodsInventoryController(ISFinishedGoodsInventory sFinishedGoodsInventory)
        {
            _sFinishedGoodsInventory = sFinishedGoodsInventory;
        }
        /// <summary>
        /// 取得成品庫存明細
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
       // [Authorize]
        [HttpPost]
        [Route("[Controller]/GetFinishedGoodsInventoryDetails")]
        public async Task<ApiResponse<VFinishGood>> GetMInventoryDetails([FromBody] MInventoryParameter filter)
        {
            VFinishGood result = new VFinishGood();
            List<VMFinishGoodDetail> list = new List<VMFinishGoodDetail>();
            try
            {
                list = await _sFinishedGoodsInventory.GetFinishedGoodsInventoryDetail(filter.CompanyID, filter);

                result.FinishGoodDetail = list;
                if (list == null)
                {
                    return new ApiResponse<VFinishGood>(new VFinishGood(), success: false, ResponseStatusCode.NoContent, "null");
                }
                return new ApiResponse<VFinishGood>(result, success: true);
                //  return result;
            }
            catch (Exception ex)
            {
                return new ApiResponse<VFinishGood>(new VFinishGood(), success: false, ResponseStatusCode.NotFound, ex.Message);
            }
          
        }
    }
}

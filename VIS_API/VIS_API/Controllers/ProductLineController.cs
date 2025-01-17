using Microsoft.AspNetCore.Mvc;
using VisLibrary.Models;
using VisLibrary.Models.View;
using VisLibrary.Service.Interface;

namespace VIS_API.Controllers
{
    /// <summary>
    /// 產線控制器
    /// </summary>
    [ApiController]
    public class ProductLineController : Controller
    {
        ISProductLine _productLineService;
        public ProductLineController(ISProductLine productLineService)
        {
            _productLineService = productLineService;
        }
        /// <summary>
        /// 取得產線資料
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[Controller]/GetProductLineList")]
        public ApiResponse<VMAproductline> GetProductLineList([FromBody] MProductLineParameter filter)
        {
            //透過Business取得產線資料
            List<Maproductline> list = new List<Maproductline>();
            try
            {
                list = _productLineService.GetProductLineList(filter);
                VMAproductline vMAproductline = new VMAproductline();
                vMAproductline.AproductlineList = list;
                if (list == null)
                {
                    return new ApiResponse<VMAproductline>(new VMAproductline(), success: false, message: "null");
                }
                return new ApiResponse<VMAproductline>(vMAproductline, success: true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<VMAproductline>(new VMAproductline(), success: false, message: ex.Message);
            }
          
        }
        //取得產線群組清單
        [HttpPost]
        [Route("[Controller]/GetProductLineGroupList")]
        public async Task<ApiResponse<VMaproductlinegp>> GetProductLineGroupList([FromBody] MaproductlinegpParamater filter)
        {
            //透過Business取得產線群組資料
            List<MMachGroup> list = new List<MMachGroup>();
            try
            {
                list = await _productLineService.GetProductLineGroupList(filter);
                VMaproductlinegp vMaproductlinegp = new VMaproductlinegp();
                vMaproductlinegp.MaproductlinegpList = list;
                if (list == null)
                {
                    return new ApiResponse<VMaproductlinegp>(new VMaproductlinegp(), success: false, message: "null");
                }
                return new ApiResponse<VMaproductlinegp>(vMaproductlinegp, success: true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<VMaproductlinegp>(new VMaproductlinegp(), success: false, message: ex.Message);
            }
           
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using VisLibrary.Models.PMD;
using VisLibrary.Service.PMD;

namespace VIS_API.Controllers.PMD
{
    /// <summary>
    /// 生產推移圖控制器
    /// </summary>
    [ApiController]
    public class ProductionShiftChartController : Controller
    {
        ISProductionShiftChart _productionShiftChartService;
        public ProductionShiftChartController(ISProductionShiftChart productionShiftChartService)
        {
            _productionShiftChartService = productionShiftChartService;
        }
        /// <summary>
        /// 取得生產推移明細
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("[Controller]/GetMWaitingfortheproduction")]
        public VMwaitingfortheproduction_details GetMWaitingfortheproduction([FromBody] WaitingfortheproductionParamater paramater)
        {
            List<Mwaitingfortheproduction_details> list = new List<Mwaitingfortheproduction_details>();
            list = _productionShiftChartService.GetProductionShiftDetailList(paramater);
            VMwaitingfortheproduction_details result = new VMwaitingfortheproduction_details();
            result.Mwaitingfortheproduction_detailList = list;
             return result;
        }
    }
}

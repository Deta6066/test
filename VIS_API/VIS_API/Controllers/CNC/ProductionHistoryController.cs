using Microsoft.AspNetCore.Mvc;
using VisLibrary.Models;
using VisLibrary.Models.CNC;
using VisLibrary.Service;
using VisLibrary.Service.CNC;
using VisLibrary.Service.Interface;

namespace VIS_API.Controllers.CNC
{
    /// <summary>
    /// 生產數量統計控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionHistoryController : Controller
    {
        ISProductionHistory _sProductionHistory;
        IAssembleCenter _sAssembleCenter;
        ISProductLine _sProductLine;

        public ProductionHistoryController(ISProductionHistory sProductionHistory, IAssembleCenter sAssembleCenter, ISProductLine sProductLine)
        {
            _sProductionHistory = sProductionHistory;
            _sAssembleCenter = sAssembleCenter;
            _sProductLine = sProductLine;
        }
      
        /// <summary>
        /// 取得生產履歷明細
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost("GetProductionHistoryDetail")]
        public async Task<ApiResponse<VMProductionHistory>> GetProductionHistoryDetail(MProductionHistoryDetailParamater filter)
        {
            VMProductionHistory vmProductionHistory = new VMProductionHistory();
            List<MProductionHistoryDetail> productionHistoryDetailList = new List<MProductionHistoryDetail>();
            List<MProductionHistory> productionHistoryList = new List<MProductionHistory>();
            List<MAssembleCenter> massemblecenterList = new List<MAssembleCenter>();
            List<MMachGroup> maproductlinegpList = new List<MMachGroup>();
            List<Maproductline> maproductlineList = new List<Maproductline>();
            CompanyParameter companyParameter = new CompanyParameter() { companyId = filter.CompanyID };
            try
            {
              
                massemblecenterList = await _sAssembleCenter.GetList(companyParameter);
                productionHistoryDetailList = await _sProductionHistory.GetProductionHistoryDetail(filter);
                productionHistoryList = await _sProductionHistory.GetProductionHistory(filter, massemblecenterList);
            
                maproductlineList = _sProductLine.GetProductLineList(new MProductLineParameter() { CompanyID = filter.CompanyID });
                maproductlinegpList = await _sProductLine.GetProductLineGroupList(new MaproductlinegpParamater() { CompanyID = filter.CompanyID.ToString() });
                vmProductionHistory.ProductionHistoryDetailList = productionHistoryDetailList;
                vmProductionHistory.ProductionHistoryList = productionHistoryList;
                vmProductionHistory.MassemblecenterList = massemblecenterList;
                vmProductionHistory.MaproductlinegpList = maproductlinegpList;
                vmProductionHistory.MaproductlineList = maproductlineList;
                return new ApiResponse<VMProductionHistory>(vmProductionHistory, success: true);
            }

            catch (Exception ex)
            {
                return new ApiResponse<VMProductionHistory>(new VMProductionHistory(), success: false, ResponseStatusCode.NotFound, ex.Message);
            }
        }
    }
}


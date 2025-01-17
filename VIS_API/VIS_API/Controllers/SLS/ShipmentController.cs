using Microsoft.AspNetCore.Mvc;
using VisLibrary.Models;
using VisLibrary.Models.View;
using VisLibrary.Service.Interface;

namespace VIS_API.Controllers.SLS
{
    /// <summary>
    /// 出貨統計表控制器
    /// </summary>
    [ApiController]
    public class ShipmentController : Controller
    {
        ISShipment _sShipment;
        public ShipmentController(ISShipment sShipment)
        {
            _sShipment = sShipment;
        }
        /// <summary>
        /// 取得出貨統計明細表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("[Controller]/GetShipmentDetailList")]
        public async Task <ApiResponse<VShipmentDetail>> GetShipmentDetailList([FromBody] MShipmentDetailParamater paramater)
        {
            VShipmentDetail vShipmentDetail = new VShipmentDetail();
            List<MShipmentDetail> list = new List<MShipmentDetail>();
            try
            {
                list = await _sShipment.GetShipmentDetailList(paramater);
                vShipmentDetail.Detail_list = list;
                if (list == null)
                {
                    return new ApiResponse<VShipmentDetail>(new VShipmentDetail(), success: false);

                }
                return new ApiResponse<VShipmentDetail>(vShipmentDetail, success: true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<VShipmentDetail>(new VShipmentDetail(), success: false, ResponseStatusCode.NotFound, ex.Message);

            }
            
            //return vShipmentDetail;
        }
    }
}

using VIS_API.Models;

namespace VIS_API.Service.Interface
{
    /// <summary>
    /// 出貨統計表服務
    /// </summary>
    public interface ISShipment
    {
        /// <summary>
        /// 取得出貨統計明細表
        /// </summary>
        /// <param name="paramater"></param>
        /// <returns></returns>
        /// 
        Task<List<MShipmentDetail>> GetShipmentDetailList(MShipmentDetailParamater paramater);
    }
}
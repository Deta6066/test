using VisLibrary.Models;

namespace VisLibrary.Service.Interface
{
    public interface ISProductLine
    {
        /// <summary>
        /// 取得產線清單
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<Maproductline> GetProductLineList(MProductLineParameter filter);
        /// <summary>
        /// 取得產線群組清單
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<List<MMachGroup>> GetProductLineGroupList(MaproductlinegpParamater filter);
    }
}
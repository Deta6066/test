using VIS_API.Models.CNC;

namespace VIS_API.Service.Interface
{
    /// <summary>
    /// 機台歷程狀態Service
    /// </summary>
    public interface ISStatus_history_info
    {
        /// <summary>
        /// 取得機台狀態歷程List
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<List<MStatus_history_info>> GetStatus_history_infoList(MStatus_history_infoParamater filter);
        /// <summary>
        /// 取得機台實時狀態List
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<List<MOperationalRate>> GetStatus_RealTime_InfoList(MStatus_history_infoParamater filter);
    }
}
using VisLibrary.Models.CNC;

namespace VisLibrary.Service.Interface
{
    /// <summary>
    /// 異常歷程統計Service
    /// </summary>
    public interface ISnomalyHistory
    {
        /// <summary>
        /// 取得異常歷程List
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<List<MAnomalyHistoryDetail>> GetAnomalyHistory(MAnomalyHistoryParameter filter,int companyID);
    }
}
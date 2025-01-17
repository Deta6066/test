using VisLibrary.Models;
using VisLibrary.Models.CNC;

namespace VisLibrary.Service.Interface
{
    /// <summary>
    /// 加工部生產履歷統計Service
    /// </summary>
    public interface ISProductionHistory
    {
        public Task<List<MProductionHistory>> GetProductionHistory(MProductionHistoryDetailParamater filter, List<MAssembleCenter> assemblecenter);
        /// <summary>
        /// 取得生產履歷統計明細
        /// </summary>
        /// <param name="filter">生產履歷統計篩選器</param>
        /// <returns></returns>

        Task<List<MProductionHistoryDetail>> GetProductionHistoryDetail(MProductionHistoryDetailParamater filter);
    }
}
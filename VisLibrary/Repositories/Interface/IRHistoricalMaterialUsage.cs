using VisLibrary.Models;
using VisLibrary.Models.PCD;

namespace VisLibrary.Repositories.Interface
{
    /// <summary>
    /// 歷史用量Repository
    /// </summary>
    public interface IRHistoricalMaterialUsage : IGenericRepositoryBase<HistoricalMaterialUsageModel> 
    {
        /// <summary>
        /// 取得歷史用料統計表
        /// </summary>
        /// <param name="filter">歷史用量篩選器模型</param>
        /// <param name="CompanyID">公司ID</param>
        /// <returns></returns>
        List<HistoricalMaterialUsageModel> GetHistoricalMaterials(MHistoricalMaterialUsageParameter filter, int CompanyID);
        /// <summary>
        /// 取得領料紀錄
        /// </summary>
        /// <returns></returns>
        Task<List<HistoricalMaterialUsageModel_Detail>> GetHistoricalMaterialUsageModel_DetailAsync(MHistoricalMaterialUsageParameter filter);
    }
}

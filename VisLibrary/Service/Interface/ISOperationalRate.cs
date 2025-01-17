using VisLibrary.Models.CNC;

namespace VisLibrary.Service.Interface
{
    /// <summary>
    /// 稼動率分析Service介面
    /// </summary>
    public interface ISOperationalRate
    {
        /// <summary>
        /// 取得稼動率分析明細
        /// </summary>
        /// <param name="filter">稼動率分析篩選器</param>
        /// <returns></returns>
        Task<List<MOperationalRate>> GetOperationalRate(MOperationalRateParamater filter);
    }
}
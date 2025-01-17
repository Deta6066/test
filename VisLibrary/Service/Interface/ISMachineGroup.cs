using VisLibrary.Models.CNC;

namespace VisLibrary.Service.Interface
{
    /// <summary>
    /// 設備群組Service
    /// </summary>
    public interface ISMachineGroup
    {
        /// <summary>
        /// 取得設備群組清單
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<List<MMachineGroup>> GetMachineGroupList(MMachineGroupParamater filter);
    }
}
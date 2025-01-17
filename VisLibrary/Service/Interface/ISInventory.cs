using VisLibrary.Models.WHE;

namespace VisLibrary.Service.Interface
{
    /// <summary>
    /// 物料庫存業務邏輯
    /// </summary>
    public interface ISInventory
    {
        /// <summary>
        /// 取得物料庫存明細
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public Task<List<MInventoryDetail>?> GetInventoryDetail(MInventoryParameter filter, int CompanyID);
    }
}

using VIS_API.Models.WHE;

namespace VIS_API.Repositories.Interface
{
    /// <summary>
    /// 物料庫存Repository
    /// </summary>
    public interface IRInventory:         IGenericRepositoryBase<MInventoryDetail>
    {
        /// <summary>
        /// 取得物料庫存明細
        /// </summary>
        /// <returns></returns>
        public List<MInventoryDetail> GetInventoryDetail(MInventoryParameter filter, int companyID=0);
        /// <summary>
        /// 新增庫存明細
        /// </summary>
        /// <param name="modelList"></param>
        /// <returns></returns>
        public bool AddInventoryDetail(List<MInventoryDetail> modelList);
    }
}

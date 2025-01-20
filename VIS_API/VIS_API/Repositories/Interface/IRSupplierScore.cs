using VIS_API.Models;

namespace VIS_API.Repositories.Interface
{
    /// <summary>
    /// 供應商達交率Repository
    /// </summary>
    public interface IRSupplierScore : IGenericRepositoryBase<MSupplierScoreDetail>
    {
        /// <summary>
        /// 透過公司ID, 供應商名稱與篩選器取得供應商達交率明細
        /// </summary>
        /// <param name="CompanyID">公司ID</param>
        /// <param name="SupplierName">供應商名稱</param>
        /// <param name="filter">達交率篩選器</param>
        /// <returns></returns>
        public List<MSupplierScoreDetail> GetSupplierDeliveryRateDetail(int CompanyID, string SupplierName, MSupplierScoreParameter filter, string cond = "1");
    }
}


using VisLibrary.Models;

namespace Service.Interface
{
    /// <summary>
    /// 供應商達交率服務介面
    /// </summary>
    public interface ISSupplierDeliveryRate
    {
        /// <summary>
        /// 取得供應商達交率
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
       Task<List<MSupplierScore>> GetSupplierDeliveryRate(int companyID, MSupplierScoreParameter filter);
        /// <summary>
        /// 取得供應商達交率明細
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
       Task<List<MSupplierScoreDetail>> GetSupplierDeliveryRateDetail(int companyID, MSupplierScoreParameter filter);
    }
}

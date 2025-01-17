using VisLibrary.Models.SLS;

namespace VisLibrary.Service.Interface
{
    /// <summary>
    /// 未交易客戶服務
    /// </summary>
    public interface ISInactiveCustomer
    {
        /// <summary>
        /// 取得未交易客戶
        /// </summary>
        /// <returns></returns>
       Task<List<MInactiveCustomer>> GetMInactiveCustomers(MInactiveCustomerParamater paramater);
    }
}
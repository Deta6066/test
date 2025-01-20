using VIS_API.Repositories.Base;
using VIS_API.Utilities;

namespace VIS_API.Models
{
    /// <summary>
    /// 公司的資料模型
    /// </summary>
    public class MCompany
    {
     

        public int ID { get; set; }
        /// <summary>
        /// 公司名稱
        /// </summary>
        public string? companyName { get; set; }
        /// <summary>
        /// 公司雲端ID
        /// </summary>
        public string companyID { get; set; }
    }
    public class CompanyParameter
    {
        [SearchKey]
        public int companyId { get; set; }

    }
}

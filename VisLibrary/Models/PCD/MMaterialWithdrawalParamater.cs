using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models.PCD
{
    /// <summary>
    /// 領料單模型篩選器
    /// </summary>
    public class MMaterialWithdrawalParamater
    {
        /// <summary>
        /// 刀庫編號或製令單號 (多筆)
        /// </summary>
        public List<string> BatchOrOrderNumber { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyID { get; set; }
    }
}

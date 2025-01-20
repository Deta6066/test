using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models.SLS
{
    /// <summary>
    /// 未交易客戶Model
    /// </summary>
    public class MInactiveCustomer
    {
        /// <summary>
        /// 客戶簡稱
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 最後交易日
        /// </summary>
        public string LastTradeDate { get; set; }
        /// <summary>
        /// 未交易天數
        /// </summary>
        public int InactiveDays { get; set; }
    }
}

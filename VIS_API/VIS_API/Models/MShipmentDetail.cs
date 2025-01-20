using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models
{
    /// <summary>
    /// 出貨明細表模型
    /// </summary>
    public class MShipmentDetail
    {
        /// <summary>
        /// 客戶簡稱
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 產線代號
        /// </summary>
        public string ProductionLineNumber { get; set; }
        /// <summary>
        /// 品號
        /// </summary>
        public string ItemNo { get; set; }
        /// <summary>
        /// 品名規格
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 小計
        /// </summary>
        public int TotalCount { get; set; }
    }
}

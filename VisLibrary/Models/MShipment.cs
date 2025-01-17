using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models
{
    /// <summary>
    /// 出貨統計表模型
    /// </summary>
    public class MShipment
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
        /// 產線名稱
        /// </summary>
        public string ProductionLineName { get; set; }
        /// <summary>
        /// 出貨日期
        /// </summary>
        public string ShipmentDate { get; set; }
        /// <summary>
        /// 出貨數量
        /// </summary>
        public int ShipmentQuantity { get; set; }
        //小計
        public int TotalCount { get; set; }
        //品號
        public string ItemNo { get; set; }

    }
}

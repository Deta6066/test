using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models.PCD
{
    /// <summary>
    /// 領料單明細模型
    /// </summary>
    public class MMaterialWithdrawalDetail
    {
        /// <summary>
        /// 領料單號
        /// </summary>
        public string MaterialWithdrawalNumber { get; set; }
        /// <summary>
        /// 領料日期
        /// </summary>
        public string MaterialWithdrawalDate { get; set; }
        /// <summary>
        /// 領料人
        /// </summary>
        public string MaterialReceiver { get; set; }
        /// <summary>
        /// 製令單號
        /// </summary>
        public string ProductionOrderNumber { get; set; }
        /// <summary>
        /// 訂單號碼
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// 使用說明
        /// </summary>
        public string UseExplain { get; set; }
        /// <summary>
        /// 倉管
        /// </summary>
        public string MaterialHandler { get; set; }
        /// <summary>
        /// 製造批號
        /// </summary>
        public string ManufacturingBatchNumber { get; set; }
        /// <summary>
        /// 客戶簡稱
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 生產元件
        /// </summary>
        public string ProductComponents { get; set; }
        /// <summary>
        /// 領料台數
        /// </summary>
        public string MaterialWithdrawalQuantityOfMachine { get; set; }
    }
}

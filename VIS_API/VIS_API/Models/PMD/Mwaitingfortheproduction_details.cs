using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models.PMD
{
    /// <summary>
    /// 生產推移圖明細
    /// </summary>
    public class Mwaitingfortheproduction_details
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
        /// 排程編號/製造批號
        /// </summary>
        public string ManufacturingBatchNumber { get; set; }
        /// <summary>
        /// 訂單號碼
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// 客戶訂單號碼
        /// </summary>
        public string CustomerOrderNumber { get; set; }
        /// <summary>
        /// 品號
        /// </summary>
        public string ItemNumber { get; set; }
        /// <summary>
        /// 品名規格
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 訂單日期
        /// </summary>
        public string? OrderDate { get; set; }
        /// <summary>
        /// 預計開工日
        /// </summary>
        public string ExpectedStartDate { get; set; }
        /// <summary>
        /// 預計完工日
        /// </summary>
        public string? ExpectedFinishDate { get; set; }
        /// <summary>
        /// 實際完成時間
        /// </summary>
        public string? FinishDate { get; set; }
        /// <summary>
        /// 製令狀態
        /// </summary>
        public string ProductionOrderStatus { get; set; }
        /// <summary>
        /// 進度
        /// </summary>
        public string? Progress { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// 組裝日
        /// </summary>
        public string AssemblyDate { get; set; }
    }
}

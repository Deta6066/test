using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Utilities;

namespace VisLibrary.Models.View
{
    /// <summary>
    /// 成品庫存明細ViewModel
    /// </summary>
    public class VMFinishGoodDetail
    {
        /// <summary>
        /// 庫存明細ID
        /// </summary>
        [SafeString]
        public int ID { get; set; }
        /// <summary>
        /// 客戶簡稱
        /// </summary>
        [SafeString]
        public string CustomerName { get; set; }
        /// <summary>
        /// 產線群組
        /// </summary>
        [SafeString]
        public string ProductionLineNumber { get; set; }
        /// <summary>
        /// 製造批號
        /// </summary>
        [SafeString]
        public string ManufacturingBatchNumber { get; set; }
        /// <summary>
        /// 入庫日
        /// </summary>
        [SafeString]
        public string ReceiptDate { get; set; }
        /// <summary>
        /// 庫存天數
        /// </summary>
        [SafeString]
        public string StockDays { get; set; }
        /// <summary>
        /// 庫存金額
        /// </summary>
        [SafeString]
        public decimal StockCost { get; set; }
        /// <summary>
        /// 庫存原因
        /// </summary>
        [SafeString]
        public string StockReason { get; set; }
        /// <summary>
        /// 廠區ID
        /// </summary>
        [SafeString]
        public string Factory_fk { get; set; } = "";
        /// <summary>
        /// 公司ID
        /// </summary>
        [SafeString]
        public string Company_fk { get; set; } = string.Empty;
    }
}

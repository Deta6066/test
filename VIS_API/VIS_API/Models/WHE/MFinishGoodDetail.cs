
using VIS_API.Utilities;

namespace VIS_API.Models.WHE
{
    /// <summary>
    /// 成品庫存明細Model
    /// </summary>
    public class MFinishGoodDetail
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
    /// <summary>
    /// 庫存明細篩選器
    /// </summary>
    public class MInventoryParameter
    {
        /// <summary>
        /// 廠區ID
        /// </summary>
        public string Factory_fk { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyID { get; set; }
        /// <summary>
        /// 庫存天數，超過此天數的庫存視為逾期庫存
        /// </summary>
        public int InventoryDays { get; set; }
        /// <summary>
        /// 倉位篩選列表
        /// </summary>
        public List<string> StorageLocationList { get; set; } = new List<string>();
        /// <summary>
        /// 類別篩選列表
        /// </summary>
        public List<string> CategoryList { get; set; } = new List<string>();
    }
}

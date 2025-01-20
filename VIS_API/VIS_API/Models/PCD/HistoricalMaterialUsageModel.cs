
using VIS_API.Utilities;

namespace VIS_API.Models.PCD
{
    /// <summary>
    /// 歷史用量模型
    /// </summary>
    public class HistoricalMaterialUsageModel
    {
        /// <summary>
        /// 品號
        /// </summary>
        [SafeString]
        public string MaterialNo { get; set; }
        /// <summary>
        /// 品名規格
        /// </summary>
        [SafeString]
        public string MaterialDescription { get; set; }
        /// <summary>
        /// 歷史組裝數量
        /// </summary>
        // public int HistoricalAssemblyQuantity { get; set; }
        /// <summary>
        /// 臥式組裝數量
        /// </summary>
        //public int HorizontalAssemblyQuantity { get; set; }
        /// <summary>
        /// 用途分類
        /// </summary>
        [SafeString]
        public Dictionary<string, int> UsageCategory { get; set; }
        /// <summary>
        /// 總計
        /// </summary>
        [SafeString]
        public int TotalUsage { get; set; }
        /// <summary>
        /// 月用量
        /// </summary>
        [SafeString]
        public int MonthlyUsage { get; set; }
        /// <summary>
        /// 安全存量
        /// </summary>
        [SafeString]
        public int SafetyStock { get; set; }
        /// <summary>
        /// 最小採購量
        /// </summary>
        [SafeString]
        public int MinimumPurchaseQuantity { get; set; }
        /// <summary>
        /// 使用客戶與數量
        /// </summary>
        [SafeString]
        public List<HistoricalMaterialUsageModel_Detail> CustomerUsage { get; set; }
        /// <summary>
        /// 產品類別
        /// </summary>
        [SafeString]
        public string ProductCategory { get; set; }
        /// <summary>
        /// 廠區ID
        /// </summary>
        [SafeString]
        public string Factory_fk { get; set; }
    }
    /// <summary>
    ///  歷史用量篩選器模型
    /// </summary>
    public class MHistoricalMaterialUsageParameter
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        [SafeString]
        public int CompanyID { get; set; }
        /// <summary>
        /// 廠區ID
        /// </summary>
        [SafeString]
        public string FactoryID { get; set; }
        /// <summary>
        /// 搜尋方式
        /// </summary>
        public string SearchType { get; set; }
        //內容篩選 ex:包含 XXX 等於XXX
        [SafeString]
        public string SearchContent { get; set; }

        /// <summary>
        /// 安全庫存量
        /// </summary>
        [SafeString]
        public double SafetyStock { get; set; }
        /// <summary>
        /// 最小採購量
        /// </summary>
        [SafeString]
        public double MinimumPurchaseQuantity { get; set; }
        /// <summary>
        /// 領料日期 開始
        /// </summary>
        [SafeString]
        public DateTime MaterialRequisitionDateStart { get; set; }
        /// <summary>
        /// 領料日期 結束
        /// </summary>
        [SafeString]
        public DateTime MaterialRequisitionDateEnd { get; set; }
        /// <summary>
        /// 品號
        /// </summary>
        [SafeString]
        public string MaterialNo { get; set; }
    }
    /// <summary>
    /// 領料記錄模型
    /// </summary>
    public class HistoricalMaterialUsageModel_Detail 
    {
        [SafeString]
        public int id { get; set; }
        /// <summary>
        /// 領料單號
        /// </summary>
        [SafeString]
        public string MaterialRequisitionID { get; set; }
        /// <summary>
        /// 領料單日期
        /// </summary>
        [SafeString]
        public string MaterialDate { get; set; }
        /// <summary>
        /// 領料單明細品號
        /// </summary>
        [SafeString]
        public string MaterialDescriptionNo { get; set; }
        /// <summary>
        /// 領料單明細品名規格
        /// </summary>
        [SafeString]
        public string MaterialDescription { get; set; }
        /// <summary>
        /// 領料數量
        /// </summary>
        [SafeString]
        public int MaterialQuantity { get; set; }
        /// <summary>
        /// 用途說明
        /// </summary>
        [SafeString]
        public string UsageDescription { get; set; }
        /// <summary>
        /// 製令單號
        /// </summary>
        [SafeString]
        public string ProductionOrderID { get; set; }
        /// <summary>
        /// 製令明細品號
        /// </summary>
        public string ProductionOrderDetailID { get; set; }
        /// <summary>
        /// 製令明細品名規格
        /// </summary>
        [SafeString]
        public string ProductionOrderDetailDescription { get; set; }
        /// <summary>
        /// 使用客戶
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 產品類別
        /// </summary>
        [SafeString]
        public string ProductCategory { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        [SafeString]
        public string Company_fk { get; set; }
    }
}

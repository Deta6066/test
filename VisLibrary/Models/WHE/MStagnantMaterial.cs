using VisLibrary.Utilities;

namespace VisLibrary.Models.WHE
{
    /// <summary>
    /// 呆滯物料明細模型
    /// </summary>
    public class MStagnantMaterial
    {

        /// <summary>
        /// 呆料明細編號
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 最後領料日
        /// </summary>
        [SafeString]
        public string LastMaterialWithdrawalDate { get; set; }
        /// <summary>
        /// 品號
        /// </summary>
        [SafeString]
        public string ItemNumber { get; set; }
        /// <summary>
        /// 品名規格
        /// </summary>
        [SafeString]
        public string ItemName { get; set; }
        /// <summary>
        /// 倉位
        /// </summary>
        [SafeString]
        public string WarehouseLocation
        { get; set; }
        /// <summary>
        /// 物料類別
        /// </summary>
        [SafeString]
        public string MaterialCategory
        { get; set; }

        /// <summary>
        /// 剩餘總庫存
        /// </summary>
        [SafeString]
        public double TotalRemainingInventory { get; set; }
        /// <summary>
        /// 領取數量
        /// </summary>
        [SafeString]
        public double MaterialWithdrawalQuantity { get; set; }
        /// <summary>
        /// 銷貨數量
        /// </summary>
        [SafeString]
        public double SalesQuantity { get; set; }
        /// <summary>
        /// 期初庫存
        /// </summary>
        [SafeString]
        public double OpeningInventory { get; set; }
        /// <summary>
        /// 領用比例
        /// </summary>
        [SafeString]
        public double MaterialWithdrawalRatio { get; set; }
        /// <summary>
        /// 標準成本
        /// </summary>
        [SafeString]
        public double StockCost { get; set; }
        /// <summary>
        /// 金額小計
        /// </summary>
        [SafeString]
        public double Subtotal { get; set; }
        /// <summary>
        /// 物料儲位
        /// </summary>
        [SafeString]
        public string GridNumber { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        [SafeString]
        public int Company_fk { get; set; }
        /// <summary>
        /// 廠區ID
        /// </summary>
        [SafeString]
        public string Factory_fk { get; set; }
    }
}

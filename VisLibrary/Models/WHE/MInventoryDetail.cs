
using VisLibrary.Utilities;

namespace VisLibrary.Models.WHE
{
    /// <summary>
    /// 庫存物料明細
    /// </summary>
    public class MInventoryDetail
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
        public string MaterialName { get; set; }
        /// <summary>
        /// 物料類別
        /// </summary>
        [SafeString]
        public string MaterialType { get; set; }
        /// <summary>
        /// 是否停用
        /// </summary>
        [SafeString]
        public string IsStop { get; set; }
        /// <summary>
        /// 倉位
        /// </summary>
        [SafeString]
        public string Warehouse { get; set; }
        /// <summary>
        /// 庫存數量
        /// </summary>
        [SafeString]
        public int InventoryQuantity { get; set; }
        /// <summary>
        /// 庫存金額
        /// </summary>
        [SafeString]
        public decimal InventoryAmount { get; set; }
    }
    public class MInventory_Total_AmountFilter
    {
        /// <summary>
        /// 廠區ID
        /// </summary>
        [SafeString]
        public string Factory_fk { get; set; }
        /// <summary>
        /// 物料類別
        /// </summary>
        [SafeString]
        public string MaterialType { get; set; } = "";
    }
}

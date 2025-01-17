using System;
using VisLibrary.Utilities;

namespace VisLibrary.Models
{
    /// <summary>
    /// 達交率模型
    /// </summary>
    public class MSupplierScore
    {
        /// <summary>
        /// 供應商名稱
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 採購次數
        /// </summary>
        public int PurchaseTimes { get; set; }
        /// <summary>
        /// 期限已交總數量
        /// </summary>
        public int OnTimeDelivery { get; set; }
        /// <summary>
        /// 採購總數量
        /// </summary>
        public int TotalPurchase { get; set; }
        /// <summary>
        /// 達交率= 期限已交總數量/採購總數量
        /// </summary>
        public double DeliveryRate { get; set; }
    }
    /// <summary>
    /// 供應商達交率明細表
    /// </summary>
    public class MSupplierScoreDetail
    {
        /// <summary>
        /// 達交率明細ID
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 供應商名稱
        /// </summary>
        public string FactoryName { get; set; }
        /// <summary>
        /// 採購單號
        /// </summary>
        public string TransactionNumber { get; set; }
        /// <summary>
        /// 採購明細序
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 品號
        /// </summary>
        public string ItemNumber { get; set; }
        /// <summary>
        /// 品名規格
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 採購數量
        /// </summary>
        public int PurchasingQuantity { get; set; }
        /// <summary>
        /// 期限已交數量
        /// </summary>
        public int DeadlineDeliveryQuantity { get; set; }
        /// <summary>
        /// 採購單日期
        /// </summary>
        public DateTime TransactionDate { get; set; }
        /// <summary>
        /// 預交日期
        /// </summary>
        public DateTime EstimatedDeliveryDate { get; set; }
        /// <summary>
        /// 交貨數量
        /// </summary>
        public int DeliveryQuantity { get; set; }
        /// <summary>
        /// 期限內最後進貨日期
        /// </summary>
        public DateTime QuantityDelivered_Deadline { get; set; }
        /// <summary>
        /// 達交率
        /// </summary>
        public double DeliveryRate { get; set; }
        /// <summary>
        /// 廠區ID
        /// </summary>
        public string Factory_fk { get; set; } = string.Empty;
        //公司ID
        public int company_fk { get; set; }
    }
    /// <summary>
    /// 供應商達交率篩選器
    /// </summary>
    public class MSupplierScoreParameter
    {
        /// <summary>
        /// 供應商名稱
        /// </summary>
        [SafeString]
        public string SupplierName { get; set; } = string.Empty;
        /// <summary>
        /// 廠區
        /// </summary>
        [SafeString]
        public string Plant { get; set; } = string.Empty;
        /// <summary>
        /// 交貨日期 開始
        /// </summary>
        public DateTime? DeliveryDateStart { get; set; }
        /// <summary>
        /// 交貨日期 結束
        /// </summary>
        public DateTime? DeliveryDateEnd { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public int company_fk { get; set; }
    }
}

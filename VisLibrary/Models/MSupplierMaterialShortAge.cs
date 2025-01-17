
using VisLibrary.Utilities;

namespace VisLibrary.Models
{
    /// <summary>
    /// 供應商未交物料模型
    /// </summary>
    public class MSupplierMaterialShortAge 
    {
        /// <summary>
        /// 催料單號
        /// </summary>
        [SafeString]
        public string UrgentMaterialNo { get; set; }
        /// <summary>
        /// 物料未交品號
        ///  </summary>
        [SafeString]
        public string ItemNumber { get; set; }
        /// <summary>
        /// 品名規格
        /// </summary>
        [SafeString]
        public string ItemName { get; set; }
        /// <summary>
        /// 需求來源
        /// </summary>
        [SafeString]
        public string DemandSource { get; set; }
        /// <summary>
        /// 需求數量
        /// </summary>
        [SafeString]
        public int MaterialRequisitionQuantity { get; set; }
        /// <summary>
        /// 客戶需求日
        /// </summary>
        public DateTime? DemandDate { get; set; }
        /// <summary>
        /// 供應商應交日
        /// </summary>
        [SafeString]
        public string DeadLineDate { get; set; }
        /// <summary>
        /// 採購/加工單號
        /// </summary>
        [SafeString]
        public string TransactionNumber { get; set; }
        /// <summary>
        /// 採購單開單日期
        /// </summary>
        public DateTime? PurchaseOrderIssueDate { get; set; }
        /// <summary>
        /// 廠商簡稱
        /// </summary>
        [SafeString]
        public string FactoryName { get; set; }
        /// <summary>
        /// 廠商代碼
        /// </summary>
        [SafeString]
        public string FactoryNumber { get; set; }
        /// <summary>
        /// 加工名稱
        /// </summary>
        [SafeString]
        public string MachiningName { get; set; }
        /// <summary>
        /// 加工代號
        /// </summary>
        [SafeString]
        public string MachiningNumber { get; set; }
        /// <summary>
        /// 已交量
        /// </summary>
        [SafeString]
        public decimal Given { get; set; }
        /// <summary>
        /// 未交量
        /// </summary>
        [SafeString]
        public decimal NotGiven { get; set; }
        /// <summary>
        /// 在外庫存
        /// </summary>
        [SafeString]
        public decimal OutQuantity { get; set; }
        /// <summary>
        /// 未如期回填繳交原因
        /// </summary>
        [SafeString]
        public string NotSuppliedReason { get; set; } = ""; //text
        /// <summary>
        /// 公司ID
        /// </summary>
        public int company_fk { get; set; } = 0;
    }
    /// <summary>
    /// 缺料查詢條件
    /// </summary>
    public class MSupplierMaterialParameter
    {
        /// <summary>
        /// 催料單號
        /// </summary>
        [SafeString]
        public string UrgentMaterialNo { get; set; }
        /// <summary>
        /// 供應商代碼
        /// </summary>
        [SafeString]
        //public string FactoryNumber { get; set; }
        public List<string> FactoryNumber { get; set; }
        /// <summary>
        /// 供應商簡稱
        /// </summary>
        [SafeString]
        public List<string> FactoryName { get; set; }
        /// <summary>
        /// 品號
        /// </summary>
        [SafeString]
        public string ItemNumber { get; set; }
        /// <summary>
        /// 催料預交日 開始
        /// </summary>
        public DateTime? UrgentMaterialDeliveryDateStart { get; set; }
        /// <summary>
        /// 催料預交日 結束
        /// </summary>
        public DateTime? UrgentMaterialDeliveryDateEnd { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyID { get; set; }

    }
}

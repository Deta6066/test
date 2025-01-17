using System.ComponentModel;
using System.Globalization;
using VisLibrary.Utilities;

namespace VisLibrary.Models
{
    public class MOrder
    {
        /// <summary>
        /// 訂單ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 客戶ID
        /// </summary>
        public string? CustomerName { get; set; }
        /// <summary>
        /// 產線ID
        /// </summary>
        public int ProductLineID { get; set; }
        /// <summary>
        /// CCS
        /// </summary>
        public string? CCS { get; set; }
        /// <summary>
        /// 製造批號
        /// </summary>
        public string? BatchNumber { get; set; }
        /// <summary>
        /// 訂單狀態
        /// </summary>
        public string? OrderStatus { get; set; }
        /// <summary>
        /// 業務員
        /// </summary>
        public string? SalesPerson { get; set; }
        /// <summary>
        /// 預交日
        /// </summary>
        public DateTime ETD { get; set; }
        /// <summary>
        /// 預計開工日
        /// </summary>
        public DateTime EWD { get; set; }
        /// <summary>
        /// 入庫日
        /// </summary>
        public DateTime storageInDay { get; set; }
        /// <summary>
        /// 出庫日
        /// </summary>
        public DateTime? storageOutDay { get; set; }
        /// <summary>
        /// 產品數量
        /// </summary>
        public decimal Count { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public int company_fk { get; set; }
        /// <summary>
        /// 廠區ID
        /// </summary>
        public string Factory_fk { get; set; } = "";
    }
    /// <summary>
    /// 客戶訂單數量統計
    /// </summary>
    public class Orders
    {
        // ID , 客戶名稱, 產線list,小計訂單數量

        public int ID { get; set; }
        public string? CustomerName { get; set; }
        /// <summary>
        /// 產線數量List
        /// </summary>
        public List<ProductLine>? ProductLines { get; set; }
        /// <summary>
        /// 小計訂單數量
        /// </summary>
        public int Count { get; set; }
    }
    // 產線list : 產線ID, 產線名稱,產線數量
    public class ProductLine
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public int Count { get; set; }
    }
    /// <summary>
    /// 訂單查詢條件
    /// </summary>
    public class OrderParameter
    {
        [SafeString]
        private DateTime _dateStart;
        [SafeString]

        private DateTime _dateEnd;

        
        /// <summary>
        /// 起始日期
        /// </summary>
        [SafeString]
        public string StartDate {
            get { return _dateStart.ToString("yyyy-MM-dd"); }
            set { _dateStart = DateTime.Parse(value); }
        }
        /// <summary>
        /// 結束日期
        /// </summary>
        [SafeString]
        public string EndDate {
            get { return _dateEnd.ToString("yyyy-MM-dd"); }
            set { _dateEnd = DateTime.Parse(value); }
        }
        /// <summary>
        /// 訂單狀態 
        /// :訂單總數,已結，未結
        /// </summary>
        [SafeString]
        public int OrderStatus { get; set; }
        /// <summary>
        /// 公司ID 
        /// </summary>
        [SafeString]
        public int Company_fk { get; set; }

        [SafeString]
        public int assemblecenter_fk { get; set; }

    }
    /// <summary>
    /// 表示包含產品線編號、客戶信息、項目詳細信息、日期和財務信息的訂單。
    /// </summary>
    public class Order 
    {
        /// <summary>
        /// 獲取或設置產品線編號。
        /// </summary>
        public string? ProductLineNo { get; set; }

        /// <summary>
        /// 獲取或設置客戶名稱。
        /// </summary>
        public string? CustomerName { get; set; }

        /// <summary>
        /// 獲取或設置項目編號。
        /// </summary>
        public string? ItemNumber { get; set; }

        /// <summary>
        /// 獲取或設置批次號。
        /// </summary>
        public string? LotNumber { get; set; }

        /// <summary>
        /// 獲取或設置訂單狀態。
        /// </summary>
        public string? OrderStatus { get; set; }

        // <summary>
        /// 獲取或設置訂單狀態的類型 0:結案 1:未結案 。
        /// </summary>
        public int? OrderStatusType { get; set; }


        /// <summary>
        /// 獲取或設置銷售員姓名。
        /// </summary>
        public string? Salesman { get; set; }

        /// <summary>
        /// 獲取或設置訂單的預計交貨日期。
        /// </summary>
        public string? EstimatedDeliveryDate { get; set; }

        /// <summary>
        /// 獲取或設置訂單的預計開始日期。
        /// </summary>
        public string? ExpectedStartDate { get; set; }

        /// <summary>
        /// 獲取或設置訂單接收日期。
        /// </summary>
        public string? ReceivedDate { get; set; }

        /// <summary>
        /// 獲取或設置訂單發貨日期。
        /// </summary>
        public string? ShipmentDate{ get; set; }

        /// <summary>
        /// 獲取或設置訂單的數量。
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 獲取或設置訂單金額。
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 獲取或設置訂單月份。
        /// </summary>
        public string? OrderMonth { get; set; }

        /// <summary>
        /// 獲取或設置產品線組。
        /// </summary>
        public string? ProductLineGroup { get; set; }

    }

   
}

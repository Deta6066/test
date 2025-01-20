using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Utilities;

namespace VIS_API.Models.WHE
{
    /// <summary>
    /// 報廢單據明細
    /// </summary>
    public class MScrapQuantity
    {
        /// <summary>
        /// 報廢明細ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 報廢者
        /// </summary>
        [SafeString]
        public string ScrapPersonnel { get; set; }
        /// <summary>
        /// 領料單號
        /// </summary>
        [SafeString]
        public string MaterialWithdrawalNumber { get; set; }
        /// <summary>
        /// 領料日期
        /// </summary>
        [SafeString]
        public string MaterialWithdrawalDate { get; set; }
        /// <summary>
        /// 品號
        /// </summary>
        [SafeString]
        public string ItemNumber { get; set; }
        /// <summary>
        /// 品名
        /// </summary>
        [SafeString]
        public string ItemName { get; set; }
        /// <summary>
        /// 單位
        /// </summary>
        [SafeString]
        public string Unit { get; set; }
        /// <summary>
        /// 庫存成本
        /// </summary>
        [SafeString]
        public double StockCost { get; set; }
        /// <summary>
        /// 庫存數量
        /// </summary>
        [SafeString]
        public double ScraQuantity { get; set; }
        /// <summary>
        /// 總報廢成本
        /// </summary>
        [SafeString]
        public double TotalScrapCost { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        [SafeString]
        public string Remark { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        [SafeString]
        public int company_fk { get; set; }





    }
}

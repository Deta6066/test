using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models.PCD
{
    //領料單模型
    public class MMaterialWithdrawal
    {
        /// <summary>
        /// 品號
        /// </summary>
        public string ItemNumber { get; set; }
        /// <summary>
        /// 品名
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 倉位
        /// </summary>
        public string WarehouseLocation { get; set; }
        /// <summary>
        /// 單位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 現有庫存
        /// </summary>
        public int Inventory { get; set; }
        /// <summary>
        /// 申請數量
        /// </summary>
        public int RequestedQuantity { get; set; }
        /// <summary>
        /// 領料數量
        /// </summary>
        public int MaterialWithdrawalQuantity { get; set; }
        /// <summary>
        /// 不足數量
        /// </summary>
        public int ShortageQuantity { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 刀庫或製令
        /// </summary>
        public string BatchOrOrderNumber { get; set; }
    }
}

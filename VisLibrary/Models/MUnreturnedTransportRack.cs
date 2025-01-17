using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisLibrary.Models
{
    /// <summary>
    /// 未歸還項目Model
    /// </summary>
    public class MUnreturnedTransportRack
    {
        //id,客戶名稱,品項,數量
        /// <summary>
        /// id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 客戶名稱
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 品項
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 正常數量
        /// </summary>
        public int NormalQuantity { get; set; }
        /// <summary>
        /// 異常數量
        /// </summary>
        public int AbnormalQuantity { get; set; }
    }
}
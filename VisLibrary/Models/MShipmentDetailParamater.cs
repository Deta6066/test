using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models
{
    /// <summary>
    /// 出貨統計表參數
    /// </summary>
    public class MShipmentDetailParamater
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyID { get; set; }
        /// <summary>
        /// 開始日期
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 廠區
        /// </summary>
        public List<string>? Area { get; set; }
    }
}

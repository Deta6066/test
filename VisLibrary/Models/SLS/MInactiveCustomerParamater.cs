using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models.SLS
{
    /// <summary>
    /// 未交易客戶參數
    /// </summary>
    public class MInactiveCustomerParamater
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyID { get; set; }
        /// <summary>
        /// 廠區
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 未交易天數
        /// </summary>
        public int InactiveDays { get; set; }
        /// <summary>
        /// 天數選項 0:小於 1:大於 
        /// </summary>
        public int InactiveDaysOption { get; set; }
        /// <summary>
        /// 開始日期
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models.CNC
{
    /// <summary>
    /// 異常歷程統計ViewModel
    /// </summary>
    public class VMAnomalyHistoryDetail
    {
        /// <summary>
        /// 設備名稱
        /// </summary>
        public string mach_name { get; set; }
        /// <summary>
        /// 異常原因
        /// </summary>

        public string alarm_mesg { get; set; }
        /// <summary>
        /// 持續時間
        /// </summary>
        public string timespan { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models.CNC
{
    /// <summary>
    /// 稼動率分析篩選器
    /// </summary>
    public class MOperationalRateParamater
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public required int company_fk { get; set; }
        /// <summary>
        /// 廠區ID
        /// </summary>
        public required int area_fk { get; set; }
        /// <summary>
        /// 設備名稱
        /// </summary>
        public string mach_name { get; set; }= string.Empty;
        /// <summary>
        /// 開始時間
        /// </summary>
        public required DateTime startTime { get; set; }
        /// <summary>
        /// 結束時間
        /// </summary>
        public required DateTime endTime { get; set; }
    }
}

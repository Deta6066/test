using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models.CNC
{
    /// <summary>
    /// 設備當前狀態資訊
    /// </summary>
    public class MStatus_currently_info
    {
        /// <summary>
        /// id
        /// </summary>
        public int _id { get; set; }
        public string mach_name { get; set; }
        /// <summary>
        /// 設備狀態
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public string update_time { get; set; }
        /// <summary>
        /// 上傳時間
        /// </summary>
        public string upload_time { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models.CNC
{
    /// <summary>
    /// 設備群組Model
    /// </summary>
    public class MMachineGroup
    {
        /// <summary>
        /// 設備群組ID
        /// </summary>
        public int _id { get; set; }
        /// <summary>
        /// 設備群組名稱
        /// </summary>
        public string group_name { get; set; }
        /// <summary>
        /// 設備名稱
        /// </summary>
        public string mach_name { get; set; }
        /// <summary>
        /// 設備顯示名稱
        /// </summary>
        public string mach_show_name { get; set; }
        /// <summary>
        /// 廠區名稱
        /// </summary>
        public string area_name { get; set; }
        /// <summary>
        /// 新增人員
        /// </summary>
        public string add_account { get; set; }
        public string web_address { get; set; }
    }
}

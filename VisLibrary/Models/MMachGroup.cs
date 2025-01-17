using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models
{
    /// <summary>
    /// 設備群組Model
    /// </summary>
    public class MMachGroup
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
        /// 設備名稱字串(多筆 用,分隔)
        /// </summary>
        public string MachName { get; set; }
        /// <summary>
        /// 設備顯示名稱字串(多筆 用,分隔)
        /// </summary>
        public string mach_show_name { get; set; }
        /// <summary>
        /// 廠區名稱
        /// </summary>
        public string area_name { get; set; }
        /// <summary>
        /// 新增人員帳號
        /// </summary>
        public string add_account { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string web_address { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        ///
        public string Company_fk { get; set; }
        /// <summary>
        /// 加工廠區ID
        /// </summary>
        public int Assemblecenter_fk { get; set; }
    }
}

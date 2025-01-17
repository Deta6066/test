using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models.CNC
{
    /// <summary>
    /// 設備資訊
    /// </summary>
    public class MMachineInfo
    {
        /// <summary>
        /// 設備ID
        /// </summary>
        public int _id { get; set; }
        /// <summary>
        /// 設備名稱
        /// </summary>
        public string mach_name { get; set; }
        /// <summary>
        /// 設備顯示名稱
        /// </summary>
        public string mach_show_name { get; set; }
        /// <summary>
        /// 設備IP
        /// </summary>
        public string mach_ip { get; set; }
        /// <summary>
        /// 設備Port
        /// </summary>
        public string mach_port { get; set; }
        /// <summary>
        /// 控制器名稱
        /// </summary>
        public string model_name { get; set; }
        /// <summary>
        /// 控制器型號
        /// </summary>
        public string model_type_name { get; set; }
        /// <summary>
        /// 廠區名稱
        /// </summary>
        public string area_name { get; set; }
        /// <summary>
        /// 設備類型
        /// </summary>
        public string mach_type { get; set; }
        /// <summary>
        /// 是否收集
        /// </summary>
        public string is_collect { get; set; }
        /// <summary>
        /// 收集次數
        /// </summary>
        public string collect_time { get; set; }
        /// <summary>
        /// 收集類型
        /// </summary>
        public string collect_type { get; set; }
        /// <summary>
        /// 攝影機地址
        /// </summary>
        public string camera_address { get; set; }
        /// <summary>
        /// 攝影機地址2
        /// </summary>
        public string camera_address_2 { get; set; }
        /// <summary>
        /// 新增人員
        /// </summary>
        public string add_account { get; set; }
        /// <summary>
        /// 圖片網址
        /// </summary>
        public string img_url { get; set; }
        /// <summary>
        /// 深度可視化
        /// </summary>
        public string deep_vis { get; set; }

    }
}

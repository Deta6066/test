using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models.CNC
{
    /// <summary>
    /// 稼動率分析Model
    /// </summary>
    public class MOperationalRate
    {
        /// <summary>
        /// 設備名稱
        /// </summary>
        public string mach_name { get; set; }
        /// <summary>
        /// 總啟動時間
        /// </summary>
        public string work_time { get; set; }
        /// <summary>
        /// 運轉時間
        /// </summary>
        public string operate_time { get; set; }
        /// <summary>
        /// 待機時間
        /// </summary>
        public string idle_time { get; set; }
        /// <summary>
        /// 警報時間
        /// </summary>
        public string alarm_time { get; set; }
        /// <summary>
        /// 離線時間
        /// </summary>
        public string disc_time { get; set; }
        /// <summary>
        /// 警告時間
        /// </summary>
        public string emergency_time { get; set; }
        /// <summary>
        /// 暫停時間
        /// </summary>
        public string suspend_time { get; set; }
        /// <summary>
        /// 手動時間
        /// </summary>
        public string manual_time { get; set; }
        /// <summary>
        /// 暖機時間
        /// </summary>
        public string warmup_time { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public string update_time { get; set; }
        /// <summary>
        /// 載入時間
        /// </summary>
        public string loading_time { get; set; }
        /// <summary>
        /// 工作日期
        /// </summary>
        public string work_date { get; set; }
        /// <summary>
        /// 運轉比例
        /// </summary>
        public string operate_rate_now { get; set; }
        /// <summary>
        /// 離線比例+++++++++++++++++                                    
        /// </summary>
        public string disc_rate_now { get; set; }
        /// <summary>
        /// 警報比例
        /// </summary>
        public string alarm_rate_now { get; set; }
        /// <summary>
        /// 待機比例
        /// </summary>
        public string idle_rate_now { get; set; }
        /// <summary>
        /// 警告比例
        /// </summary>
        public string emergency_rate_now { get; set; }
        /// <summary>
        /// 暫停比例
        /// </summary>
        public string suspend_rate_now { get; set; }
        /// <summary>
        /// 手動比例
        /// </summary>
        public string manual_rate_now { get; set; }
        /// <summary>
        /// 暖機比例
        /// </summary>
        public string warmup_rate_now { get; set; }

    }
}

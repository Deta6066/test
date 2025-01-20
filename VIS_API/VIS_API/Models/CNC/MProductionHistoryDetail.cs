using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models.CNC
{
    /// <summary>
    /// 生產履歷統計明細模型
    /// </summary>
    public class MProductionHistoryDetail
    {
        /// <summary>
        /// 機台ID
        /// </summary>
        public string MachineID { get; set; }
        /// <summary>
        /// 機台名稱
        /// </summary>
        public string MachineName { get; set; }
        /// <summary>
        /// 工藝名稱/運行程式
        /// </summary>
        public string RunProgram { get; set; }
        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 加工時間
        /// </summary>
        public decimal ProcessTime { get; set; }


    }
}

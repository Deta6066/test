using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models.CNC
{
    /// <summary>
    /// 異常歷程統計參數
    /// </summary>
    public class MAnomalyHistoryParameter
    {
        /// <summary>
        /// 廠區ID
        /// </summary>
        public int AreaID { get; set; }
        /// <summary>
        /// 設備名稱
        /// </summary>
        public string MachName { get; set; 
        }
        /// <summary>
        /// 開始時間
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 結束時間
        /// </summary>
        public string EndDate { get; set; }
    }
}

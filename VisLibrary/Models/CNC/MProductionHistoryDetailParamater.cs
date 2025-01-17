using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models.CNC
{
    /// <summary>
    /// 生產履歷篩選器
    /// </summary>
    public class MProductionHistoryDetailParamater
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public required int CompanyID { get; set; }
        /// <summary>
        /// 廠區ID
        /// </summary>
        public required int AreaID { get; set; }
        // 廠區群組List
        public required List<int> AreaGroupIDList { get; set; }
        /// <summary>
        /// 開始時間
        /// </summary>
        public required DateTime StartTime { get; set; }
        /// <summary>
        /// 結束時間
        /// </summary>
        public required DateTime EndTime { get; set; }
    }
}

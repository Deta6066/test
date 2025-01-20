using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models.CNC
{
    public class VMStatusHistory_info
    {
        public List<MStatus_history_info> mStatus_History_InfoList { get; set; }
        /// <summary>
        /// 當前設備實時狀態
        /// </summary>
        public List<MOperationalRate> mStatus_RealTime_InfoList { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models.CNC;

namespace VisLibrary.Models.View
{
    /// <summary>
    /// 設備監控看板ViewModel
    /// </summary>
    public class VMachine_info
    {
        /// <summary>
        /// 設備資訊
        /// </summary>
        public List<MMachineInfo> MachineInfo { get; set; }
        /// <summary>
        /// 可視化資訊
        /// </summary>
        public List<MAps_info> ApsInfo { get; set; }
        /// <summary>
        /// 設備當前狀態資訊
        /// </summary>
        public List<MStatus_currently_info> StatusCurrentlyInfo { get; set; }
        /// <summary>
        /// 設備群組資訊
        /// </summary>
        public List<MMachineGroup> MMachineGroupList { get; set; }
        /// <summary>
        /// 設備圖片資訊
        /// </summary>
        public List<VMMachineImage> MMachineImageList { get; set; }
    }
}

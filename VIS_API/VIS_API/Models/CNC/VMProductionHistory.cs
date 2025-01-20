using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models.CNC
{
    /// <summary>
    /// 生產履歷統計明細ViewModel
    /// </summary>
    public class VMProductionHistory
    {
        /// <summary>
        /// 生產履歷數量統計明細List
        /// </summary>
        public List<MProductionHistoryDetail> ProductionHistoryDetailList { get; set; }
        /// <summary>
        /// 生產履歷數量統計List
        /// </summary>
        public List<MProductionHistory> ProductionHistoryList { get; set; }
        /// <summary>
        /// 廠區List
        /// </summary>
        public List<MAssembleCenter> MassemblecenterList { get; set; }
        /// <summary>
        /// 產線群組List
        /// </summary>
        public List<MMachGroup> MaproductlinegpList { get; set; }
        /// <summary>
        /// 產線List
        /// </summary>
        public List<Maproductline> MaproductlineList { get; set; }
    }
}

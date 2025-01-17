using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models.CNC
{
    /// <summary>
    /// 生產履歷統計Model
    /// </summary>
    public class MProductionHistory
    {
        /// <summary>
        /// 設備名稱
        /// </summary>
        public string MachineID { get; set; }
        /// <summary>
        /// 工藝名稱(運行程式)
        /// </summary>
        public string RunProgram { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 金額
        /// </summary>
        public decimal Amount { get; set; }
    }
}

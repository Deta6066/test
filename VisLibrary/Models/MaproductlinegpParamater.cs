using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models
{
    /// <summary>
    /// 產線群組篩選器
    /// </summary>
    public class MaproductlinegpParamater
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public string CompanyID { get; set; }
        /// <summary>
        /// 加工廠區ID
        /// </summary>
        public int AssemblecenterID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models.CNC
{
    public class MStatus_history_infoParamater
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyID { get; set; }
        /// <summary>
        /// 機台名稱
        /// </summary>
        public string mach_name { get; set; }
        /// <summary>
        /// 選擇日期
        /// </summary>
        public DateTime select_date { get; set; }
    }
}

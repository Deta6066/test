using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models
{
    /// <summary>
    /// 未歸還項目篩選器
    /// </summary>
    public class MUnreturnedTransportRackParamater
    {
        /// <summary>
        /// 廠區代號
        /// </summary>
        public string FactoryID { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyID { get; set; }

    }
}

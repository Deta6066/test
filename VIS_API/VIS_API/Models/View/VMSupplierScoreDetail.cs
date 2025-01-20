using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models.View
{
    /// <summary>
    /// 供應商達交率ViewModel
    /// </summary>
    public class VMSupplierScoreDetail
    {
        /// <summary>
        /// 達交率明細列表
        /// </summary>
       public List<MSupplierScoreDetail> SupplierScoreDetailList { get; set; }
    }
}

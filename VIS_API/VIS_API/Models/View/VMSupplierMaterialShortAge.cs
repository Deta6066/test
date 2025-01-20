using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models.View
{
    /// <summary>
    /// 未出貨統計ViewModel
    /// </summary>
    public class VMSupplierMaterialShortAge
    {
        /// <summary>
        /// 供應商未交物料
        /// </summary>
        public List<MSupplierMaterialShortAge> SupplierScoreDetailList { get; set; }
    }
}

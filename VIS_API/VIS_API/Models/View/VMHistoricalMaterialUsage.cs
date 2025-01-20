using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Models.PCD;

namespace VIS_API.Models.View
{
    /// <summary>
    /// 歷史用量ViewModel
    /// </summary>
    public class VMHistoricalMaterialUsage
    {
        /// <summary>
        /// 歷史用量明細
        /// </summary>
       public List<HistoricalMaterialUsageModel_Detail> HistoricalMaterialUsageModelList { get; set; }
    }
}

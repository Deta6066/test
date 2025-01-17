using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models.CNC
{
    /// <summary>
    /// 異常歷程統計ViewModel
    /// </summary>
    public class VMAnomalyHistory
    {
        /// <summary>
        /// 異常歷程統計List
        /// </summary>
       public List<VMAnomalyHistoryDetail> MAnomalyHistories { get; set; }= new List<VMAnomalyHistoryDetail>();

    }
}

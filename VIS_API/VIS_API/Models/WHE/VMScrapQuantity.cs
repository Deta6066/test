using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models.WHE
{
    /// <summary>
    /// 報廢數量統計表ViewModel
    /// </summary>
    public class VMScrapQuantity
    {
        /// <summary>
        /// 報廢數量統計List
        /// </summary>
       public List<MScrapQuantity> MScrapQuantityList { get; set; }
    }
}

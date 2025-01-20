using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Models.SLS;

namespace VIS_API.Models.View
{
    /// <summary>
    /// 未交易客戶ViewModel
    /// </summary>
    public class VMInactiveCustomer
    {
        /// <summary>
        /// 未交易客戶List
        /// </summary>
       public List<MInactiveCustomer> MInactiveCustomerList { get; set; }
    }
}

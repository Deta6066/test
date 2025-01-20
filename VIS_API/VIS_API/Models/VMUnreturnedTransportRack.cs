using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models
{
    /// <summary>
    /// 未歸還項目ViewModel
    /// </summary>
    public class VMUnreturnedTransportRack
    {
      public required List<MUnreturnedTransportRack>  UnreturnedTransportRackList { get; set; }
    }
}

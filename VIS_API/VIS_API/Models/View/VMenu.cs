using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Models.JsonModel;

namespace VIS_API.Models.View
{
    public class VMenu
    {
        public List<MMenu>? Menu { get; set; }
        public List<MAllowMenu>? allowMenu { get; set; }
    }
}

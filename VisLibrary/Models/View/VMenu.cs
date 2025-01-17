using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models.JsonModel;

namespace VisLibrary.Models.View
{
    public class VMenu
    {
        public List<MMenu>? Menu { get; set; }
        public List<MAllowMenu>? allowMenu { get; set; }
    }
}

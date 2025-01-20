using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Utilities;

namespace VIS_API.Models.View
{
    public class VProductGp
    {
       
        public required string id { get; set; }
        public string? sID { get; set; }
        public string? sName { get; set; }
        public required string Company_fk { get; set; }
        public string? Productlinegp_fk { get; set; }
        public required string plId { get; set; }
    }
}

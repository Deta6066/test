using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models
{
    public class MCompanySetting
    {
        public int company_fk { get; set; }
        public int? start_day { get; set; }
        public int? end_day { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
    }
}

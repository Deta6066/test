using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models.CNC
{
    public class MStatus_history_info
    {
        public int _id { get; set; }
        public string mach_name { get; set; }
        public string status { get; set; }
        public string update_time { get; set; }
        public string enddate_time { get; set; }
        public string timespan { get; set; }
        public string upload_time { get; set; }
        public string Type { get; set; }
        public string Detail { get; set; }

    }
}

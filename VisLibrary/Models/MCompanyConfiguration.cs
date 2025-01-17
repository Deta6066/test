using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models
{
    public class MCompanyConfiguration
    {
        public int company_fk { get; set; } 
        public int? start_day { get; set; }
        public int? end_day { get; set; }

        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
    }

    public class VDateRange

    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

    public class CompanyConfigurationParameter
    {
        public int CompanyId { get; set; }
    }
}

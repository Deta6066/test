using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models.JsonModel
{
    public class ProcessWorkLogModel
    {
        public int? log_id { get; set; }

        public string? worker_id { get; set; }
        public string? worker_name { get; set; }

        public string? process_id { get; set; }

        public string? schedule_number { get; set; }

        public string? item_number { get; set; }
        public int month { get; set; }
        public DateTimeOffset? start_time { get; set; }

        public DateTimeOffset? end_time { get; set; }

        public decimal? points_earned { get; set; }

        public decimal? punish_point { get; set; }

        public string? remarks { get; set; }
    }
}

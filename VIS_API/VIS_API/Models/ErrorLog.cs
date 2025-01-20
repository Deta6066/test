using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Utilities;

namespace VIS_API.Models
{
    public class ErrorLog
    {
        public required string Message { get; set; }
        public required string? StackTrace { get; set; }
        public DateTime Timestamp { get; set; }
        public required string SourceIp { get; set; }
    }
}

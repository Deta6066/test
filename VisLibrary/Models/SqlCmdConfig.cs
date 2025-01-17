using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models
{
    public class SqlCmdConfig
    {
        public  CRUD? assemblecenter { get; set; }
        public CRUD? productionLineGroup { get; set; }
        public CRUD? companydatasource { get; set; }

    }

    public class CRUD
    {
        public string? Get { get; set; }
        public string? Insert { get; set; }
        public string? Update { get; set; }
        public string? Delete { get; set; }
    }
}

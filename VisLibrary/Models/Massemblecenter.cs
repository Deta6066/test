using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Repositories.Base;
using VisLibrary.Utilities;

namespace VisLibrary.Models
{
    /// <summary>
    /// 加工中心(廠區)model
    /// </summary>
    public class MAssembleCenter
    {
        
        public  int id {  get; set; }
        public  string? sID { get; set; }
        public  string? sName { get; set; }
        [SearchKey]
        public required int Company_fk { get; set; }
    }
    public class VAssembleCenter
    {
        public int? Value { get; set; }
        public string? Label { get; set; }
    }
}

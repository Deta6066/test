using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models.CNC
{
    /// <summary>
    /// 上傳圖片參數
    /// </summary>
    public class MMachineImageParameter
    {
        public int Company_fk { get; set; }
        public int Area_fk { get; set; }
        public string MachineID { get; set; }
    }
}

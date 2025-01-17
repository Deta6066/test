using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models.CNC
{
    /// <summary>
    /// 工單資訊
    /// </summary>
    public class MWorkOrder
    {
        //工單ID

        /// <summary>
        /// 設備名稱
        /// </summary>
        public string mach_name { get; set; }
        /// <summary>
        /// 操作人員
        /// </summary>
        public string work_staff { get; set; }
        /// <summary>
        /// 製令單號
        /// </summary>
        public string manu_id { get; set; }
        /// <summary>
        /// 客戶名稱
        /// </summary>
        public string custom_name { get;set; }
        //產品名稱
        public string product_name { get; set; }
        //料件編號
        public string product_number { get; set; }
        //工藝名稱
        public string craft_name { get; set; }
        //實際生產數量

        //預計生產數量
    }
}

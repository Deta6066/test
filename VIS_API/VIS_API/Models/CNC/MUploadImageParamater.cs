using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models.CNC
{
    /// <summary>
    /// 上傳圖片參數
    /// </summary>
    public class MUploadImageParamater
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public string Company_fk { get; set; }
        /// <summary>
        /// 加工廠區ID
        /// </summary>
        public int Area_fk { get; set; }
        /// <summary>
        /// 設備ID
        /// </summary>
        public string MachineID { get; set; }
        /// <summary>
        /// 圖片檔案
        /// </summary>
        public IFormFile ImageFile { get; set; }
    }
}

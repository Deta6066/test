using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models.CNC
{
    /// <summary>
    /// 設備圖片ViewModel
    /// </summary>
    public class VMMachineImage
    {
        public int id { get; set; }

        public string mach_id { get; set; }
        /// <summary>
        /// 圖片資料
        /// </summary>
        public byte[] image_data { get; set; }
        /// <summary>
        /// 圖片資料base64字串
        /// </summary>
        public string image_data_base64 { get; set; }
        /// <summary>
        /// 圖片所屬類型 ex:1:設備圖片 2:加工圖片
        /// </summary>
        public int image_type { get; set; }
        /// <summary>
        /// 所屬公司ID
        /// </summary>

        public string company_fk { get; set; }
        /// <summary>
        /// 所屬廠區ID
        /// </summary>
        public string area_fk { get; set; }
    }
}

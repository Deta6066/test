using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Repositories.Base;

namespace VIS_API.Models.CNC
{
    /// <summary>
    /// 異常歷程統計Model
    /// </summary>
    public class MAnomalyHistoryDetail
    {
        /// <summary>
        /// 異常歷程統計ID
        /// </summary>
        [PrimaryKey]
        [SearchKey]
        [InsertIgnore]
        public int _id { get; set; }
        /// <summary>
        /// 設備名稱
        /// </summary>
        public string mach_name { get; set; }
        /// <summary>
        /// 異常編號
        /// </summary>
        public string alarm_num { get; set; }
        /// <summary>
        /// 異常類型
        /// </summary>
        public string alarm_type { get; set; }
        /// <summary>
        /// 異常訊息
        /// </summary>
        public string alarm_mesg { get; set; }
        /// <summary>
        /// 開始時間
        /// </summary>
        public string update_time { get; set; }
        /// <summary>
        /// 結束時間
        /// </summary>
        public string enddate_time { get; set; }
        /// <summary>
        /// 持續時間
        /// </summary>
        public string timespan { get; set; }
        /// <summary>
        /// 上傳時間
        /// </summary>
        public string upload_time { get; set; }
        /// <summary>
        /// 所屬公司ID
        /// </summary>
        public int Company_fk { get; set; }
        /// <summary>
        /// 所屬廠區ID
        /// </summary>
        public int Area_fk { get; set; }

    }
}

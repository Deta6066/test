using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models.PMD
{
    /// <summary>
    /// 生產推移圖明細參數
    /// </summary>
    public class WaitingfortheproductionParamater
    {
        //廠區
        public string Factory { get; set; }
        /// <summary>
        /// 圖片選擇
        /// 生產推移圖，生產領料圖 
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 開始日期
        /// </summary>
        public string startDate { get; set; }
        /// <summary>
        /// 結束日期
        /// </summary>
        public string endDate { get; set; }
        /// <summary>
        /// 選擇產線
        /// </summary>
        List<string> ProductionLine { get; set; }

        /// <summary>
        /// 公司代號
        /// </summary>
        public int CompanyID { get; set; }
    }
}

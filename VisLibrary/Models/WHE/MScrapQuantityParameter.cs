using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Utilities;

namespace VisLibrary.Models.WHE
{
    /// <summary>
    /// 報廢數量統計篩選器
    /// </summary>
    public class MScrapQuantityParameter
    {
        /// <summary>
        /// 廠區
        /// </summary>
        [SafeString]
        public string Factory_fk { get; set; }
        //日期區間
        [SafeString]
        public DateTime StartDate { get; set; }
        [SafeString]
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 報廢人員清單
        /// </summary>
        [SafeString]
        public List<string>? ScrapPersonnel { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyID { get; set; }
    }
}

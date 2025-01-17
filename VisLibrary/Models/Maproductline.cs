using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models
{
    /// <summary>
    /// 產線資料模型
    /// </summary>
    public class Maproductline
    {
        public int id { get; set; }
        /// <summary>
        /// 產線ID
        /// </summary>
        public string sID { get; set; }
        /// <summary>
        /// 產線名稱
        /// </summary>
        public string sName { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public string Company_fk { get; set; }
        /// <summary>
        /// 產線群組ID
        /// </summary>
        public int Productlinegp_fk { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIS_API.Models
{
    /// <summary>
    /// Api資訊
    /// </summary>
    public class MApiInfo
    {
        /// <summary>
        /// api名稱
        /// </summary>
        public required string ApiName { get; set; }
        /// <summary>
        /// api網址
        /// </summary>
        public required string ApiUrl { get; set; }
        /// <summary>
        /// api方法
        /// </summary>
        public required string ApiMethod { get; set; } = "post";
    }
}

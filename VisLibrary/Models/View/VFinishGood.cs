using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models.WHE;

namespace VisLibrary.Models.View
{
    /// <summary>
    /// 成品庫存視圖模型
    /// </summary>
    public class VFinishGood
    {
        /// <summary>
        /// 成品庫存明細
        /// </summary>
        public List<VMFinishGoodDetail>? FinishGoodDetail { get; set; }
    }
}

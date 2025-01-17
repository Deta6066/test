using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models
{
    public class SqlInfo
    {
        public required string Orders { get; set; }
        public required string Orders_Finished { get; set; }
        public required string Orders_Unfinished { get; set; }
        public required string Orders_Scheduled { get; set; }
        public required string Orders_Unscheduled { get; set; }
        public required string Orders_Stock { get; set; }
        public required string Orders_Unstock { get; set; }

        public required string OrderOverdueList { get; set; }
        public required string GetMachineInfo { get; set; }
        /// <summary>
        /// 取得成品庫存明細
        /// </summary>
        public string? GetFinsihedGoodsInventoryDetail { get; set; }
        /// <summary>
        /// 取得庫存數量明細
        /// </summary>
        public required string GetInventoryDetail { get; set; }
        /// <summary>
        /// 取得呆滯物料明細
        /// </summary>
        public string? GetStagnantMaterialDetail { get; set; }
        /// <summary>
        /// 取得報廢數量明細
        /// </summary>
        public string? GetScrapQuantityList { get; set; }
        /// <summary>
        /// 取得生產推移圖明細
        /// </summary>
        public string? GetProductionShiftDetail { get; set; }
    }
}

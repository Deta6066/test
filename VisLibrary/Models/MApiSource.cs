using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models
{
    /// <summary>
    /// Api資料來源Model
    /// </summary>
    public class MApiSource
    {
        /// <summary>
        /// 訂單Api
        /// </summary>
        public MApiInfo? Order { get; set; }
        public MApiInfo? OrderDetail { get; set; }
        /// <summary>
        /// 取得報廢數量統計api
        /// </summary>
        public MApiInfo? GetScrapQuantityList { get; set; }
        /// <summary>
        /// 取得呆滯物料統計表api
        /// </summary>
        public MApiInfo? GetStagnantMaterialList { get; set; }
        /// <summary>
        /// 取得供應商達交率明細api
        /// </summary>
        public MApiInfo? GetSupplierDeliveryRateDetail { get; set; }
        /// <summary>
        /// 取得成品庫存明細api
        /// </summary>
        public required MApiInfo  GetFinishedGoodsInventoryDetails { get;set; }
        /// <summary>
        /// 取得物料庫存總量api
        /// </summary>
        public required MApiInfo GetInventoryDetail { get; set; }
        /// <summary>
        /// 取得供應商物料未交物料api
        /// </summary>
        public MApiInfo? GetSupplierMaterialShortAge { get; set; }
        /// <summary>
        /// 取得領料記錄api
        /// </summary>
        public MApiInfo? GetHistoricalMaterialUsageModel_Detail { get; set; }
    }
}

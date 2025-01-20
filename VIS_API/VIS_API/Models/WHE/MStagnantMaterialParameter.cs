using VIS_API.Utilities;

namespace VIS_API.Models.WHE
{
    /// <summary>
    /// 呆滯物料篩選器
    /// </summary>
    public class MStagnantMaterialParameter
    {
        /// <summary>
        /// 廠區
        /// </summary>
        [SafeString]
        public string Factory_fk { get; set; }
        /// <summary>
        /// 存放儲位
        /// </summary>
        [SafeString]
        public List<string> WarehouseLocation { get; set; }
        /// <summary>
        /// 庫存天數
        /// </summary>
        [SafeString]
        public int InventoryDays { get; set; }
        /// <summary>
        /// 物料類別
        /// </summary>
        [SafeString]
        public List<string> MaterialCategory { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        [SafeString]
        public int CompanyID { get; set; }
        /// <summary>
        /// 報廢開始日期
        /// </summary>
        [SafeString]
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 報廢結束日期
        /// </summary>
        [SafeString]
        public DateTime? EndDate { get; set; }
    }
}

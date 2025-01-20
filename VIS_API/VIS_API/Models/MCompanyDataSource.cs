
using VIS_API.Repositories.Base;

namespace VIS_API.Models
{
    /// <summary>
    /// 公司資料來源Model
    /// </summary>
    public class MCompanyDataSource
    {
        #region
        /// <summary>
        /// 資料來源ID
        /// </summary>
        [InsertIgnore]
        [UpdateIgnore]
        [PrimaryKey]
        public int ID { get; set; } = 0; // int(10) unsigned
        /// <summary>
        /// 公司ID
        /// </summary>
        [SearchKey]
        public int company_fk { get; set; } = 0; // int(10) unsigned
        /// <summary>
        /// 資料來源類型 1:內部資料庫 2:外部API 3:外部檔案
        /// </summary>
        [UpdateIgnore]
        public int sourceType { get; set; } = 0; // tinyint(4) 
        /// <summary>
        /// json路徑
        /// </summary>
        [UpdateIgnore]
        public string? apiSource { get; set; }  // text
        /// <summary>
        /// 資料庫參數
        /// </summary>
        [UpdateIgnore]

        public string? dbParamater { get; set; }  // text
        /// <summary>
        /// api來源
        /// </summary>
        [UpdateIgnore]
        public string? sqlcmd { get; set; }  // text
        /// <summary>
        /// DB類型 0:MySQL 1:MsSQL
        /// </summary>
        public int dbType { get; set; }
        #endregion
    }
}

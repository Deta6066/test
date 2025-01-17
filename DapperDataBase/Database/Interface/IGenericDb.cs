using System.Data.Common;

namespace DapperDataBase.Database.Interface
{
    /// <summary>
    /// 定義通用資料庫操作的介面。
    /// </summary>
    public interface IGenericDb : IDb
    {
        /// <summary>
        /// 獲取資料庫連接類型。
        /// </summary>
        /// <returns>資料庫連接類型。</returns>
        public string GetDbConnectionType();
        public string GetDbConnectionString();
        DbConnection GetDbConnection(); // 新增的方法
        void UpdateConnectionInfo(string connectionString,int dbType);
        void UpdateConnectionInfo(DbConnection newConnection);


    }
}
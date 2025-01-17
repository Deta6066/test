
using System.Data.Common;

namespace DapperDataBase.Database.Interface
{
    /// <summary>
    /// 定義資料庫工廠介面，用於創建資料庫連接。
    /// </summary>
    public interface IDbFactory
    {
        
        /// <summary>
        /// 根據資料庫類型創建資料庫連接。
        /// </summary>
        /// <param name="dbType">資料庫類型。</param>
        /// <returns>資料庫連接。</returns>
        //IGenericDb Create(string dbType);
        //IGenericDb Create(string ConnectionString, int dbType);
        DbConnection CreateConnection(string dbType);
        DbConnection CreateConnection(string ConnectionString, int dbType);


    }
}

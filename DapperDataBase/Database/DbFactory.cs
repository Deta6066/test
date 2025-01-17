using Microsoft.Extensions.Configuration;
using MySqlConnector;
using DapperDataBase.Database.Interface;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace DapperDataBase.Database
{
    /// <summary>
    /// 資料庫工廠實現，用於創建資料庫連接。
    /// </summary>
    public class DbFactory : IDbFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 初始化DbFactory類的新實例。
        /// </summary>
        /// <param name="serviceProvider">服務提供者。</param>
        /// <param name="configuration">配置提供者。</param>
        public DbFactory(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        /// <summary>
        /// 根據資料庫類型創建資料庫連接。
        /// dbType=mysql 從appsetting.json取得連線字串 連接mysql。
        /// dbType=mssql 從appsetting.json取得連線字串 連接mssql。
        /// </summary>
        /// <param name="dbType">資料庫類型。</param>
        /// <returns>資料庫連接。</returns>
        //public IGenericDb Create(string dbType)
        //{
        //    switch (dbType.ToLower())
        //    {
        //        case "mysql":
        //            var mySqlConnString = _configuration.GetConnectionString("MySqlConnectionString");
        //            if (mySqlConnString is null)
        //            {
        //                throw new ArgumentNullException(mySqlConnString, "mySqlConnString is null.");
        //            }
        //            DbConnection connection;

        //            connection = new MySqlConnection(mySqlConnString);
        //            return new GenericDb(connection, "mysql", this);
        //        case "mssql":
        //            var msSqlConnString = _configuration.GetConnectionString("MsSqlConnectionString");
        //            if (msSqlConnString is null)
        //            {
        //                throw new ArgumentNullException(msSqlConnString, "msSqlConnString is null.");
        //            }
        //            DbConnection connection2;

        //            connection2 = new SqlConnection(msSqlConnString);
        //            return new GenericDb(connection2, "mssql", this);
        //        default:
        //            throw new KeyNotFoundException($"Database key '' not found.");
        //    }
        //}
        /// <summary>
        /// 根據資料庫連接信息創建資料庫連接
        /// </summary>
        /// <param name="ConnectionString">資料庫連線字串。</param>
        /// <param name="dbType">資料庫類別。</param>

        /// <returns>資料庫連接。</returns>
        //public IGenericDb Create(string ConnectionString, int dbType)
        //{
        //    var connectionString = ConnectionString ?? "";

        //    switch (dbType)
        //    {
        //        case 0:
        //            DbConnection connection;

        //           return connection = new MySqlConnection(connectionString);

        //            return new GenericDb(connection, "mysql", this);
        //        case 1:
        //            DbConnection connection2;

        //            connection2 = new SqlConnection(connectionString);
        //            return new GenericDb(connection2, "mssql",this);
        //        default:
        //            throw new KeyNotFoundException($"Database key '' not found.");
        //    }
        //}

        public DbConnection CreateConnection(string dbType)
        {
            switch (dbType.ToLower())
            {
                case "mysql":
                    var mySqlConnString = _configuration.GetConnectionString("MySqlConnectionString");
                    if (mySqlConnString is null)
                    {
                        throw new ArgumentNullException(mySqlConnString, "mySqlConnString is null.");
                    }

                    return new MySqlConnection(mySqlConnString);
                case "mssql":
                    var msSqlConnString = _configuration.GetConnectionString("MsSqlConnectionString");
                    if (msSqlConnString is null)
                    {
                        throw new ArgumentNullException(msSqlConnString, "msSqlConnString is null.");
                    }

                    return new SqlConnection(msSqlConnString);
                default:
                    throw new KeyNotFoundException($"Database key '' not found.");
            }
        }

        public DbConnection CreateConnection(string ConnectionString, int dbType)
        {
            var connectionString = ConnectionString ?? "";

            switch (dbType)
            {
                case 0:

                    return new MySqlConnection(connectionString);
                case 1:

                    return new SqlConnection(connectionString);
                default:
                    throw new KeyNotFoundException($"Database key '' not found.");
            }
        }
    }
}

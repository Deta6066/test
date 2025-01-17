using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using DapperDataBase.Database.Interface;

namespace DapperDataBase.Database
{
    /// <summary>
    /// 通用資料庫操作類別。
    /// </summary>
    public class GenericDb : BaseDAL, IGenericDb
    {
        private readonly ITransactionManager _transactionManager;
        private DbConnection _connection;

        /// <summary>
        /// 初始化 GenericDb 類的新實例。
        /// </summary>
        ///<param name="connection">資料庫連線物件</param>
        ///<param name="transactionManager">交易模式物件</param>
        public GenericDb(DbConnection connection ,ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager ?? throw new ArgumentNullException(nameof(transactionManager));
            _connection = connection;
        }

        /// <summary>
        /// 更新資料庫連接信息。
        /// </summary>
        /// <param name="connectionInfo">新的資料庫連接信息。</param>
        /// 棄用
        public void UpdateConnectionInfo(string connectionString, int dbType)
        {
            throw new NotImplementedException(); 
        }

        /// <summary>
        /// 更新資料庫連接信息。
        /// </summary>
        /// <param name="connectionString">資料庫連線字串。</param>
        /// <param name="dbType">資料庫類型。</param>
        public void UpdateConnectionInfo(DbConnection newConnection)
        {
            if (newConnection == null)
                throw new ArgumentNullException(nameof(newConnection));

            // 釋放現有連線
            //if (Connection != null)
            //{
            //    if (Connection.State == ConnectionState.Open)
            //        Connection.Close();
            //    Connection.Dispose();
            //}
            _connection = newConnection;
            // 更新 BaseDAL 中的連線
            //SetConnection(newConnection);            
        }

        /// <summary>
        /// 獲取資料庫連接類型。
        /// </summary>
        /// <returns>資料庫連接類型。</returns>
        /// 棄用
        public string GetDbConnectionType()
        {
            return "null";
        }

        /// <summary>
        /// 獲取資料庫連接字串。
        /// </summary>
        /// <returns>資料庫連接字串。</returns>
        /// 棄用
        public string GetDbConnectionString()
        {
            return _connection.ConnectionString;
        }

        /// <summary>
        /// 獲取資料庫連接。
        /// </summary>
        /// <returns>資料庫連接。</returns>
        /// 棄用
        public DbConnection GetDbConnection()
        {
            return _connection;
        }




        public async Task<int> ExecuteNonQueryAsync(string sql, DynamicParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new ArgumentException("SQL cannot be null or empty", nameof(sql));

            return await ExecuteWithTransactionAsync(async (conn, trans) =>
            {
                Console.WriteLine($"Executing SQL: {sql}");
                return await conn.ExecuteAsync(new CommandDefinition(sql, parameters, trans, cancellationToken: cancellationToken));
            });
        }

        public async Task<T?> ExecuteScalarAsync<T>(string sql, DynamicParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new ArgumentException("SQL cannot be null or empty", nameof(sql));

            return await ExecuteWithTransactionAsync(async (conn, trans) =>
            {
                Console.WriteLine($"Executing SQL: {sql}");
                return await conn.ExecuteScalarAsync<T>(new CommandDefinition(sql, parameters, trans, cancellationToken: cancellationToken));
            });
        }

        public async Task<List<T>> GetListAsync<T>(string sql, DynamicParameters? parameters = null, int limit = 0, CancellationToken cancellationToken = default) where T : class
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new ArgumentException("SQL cannot be null or empty", nameof(sql));

            // 判斷資料庫類型
            string dbType = GetDatabaseType();

            if (limit > 0)
            {
                if (dbType == "SQLServer")
                {
                    // SQL Server 使用 TOP 子句
                    sql = InsertTopClause(sql, limit);
                }
                else if (dbType == "MySQL")
                {
                    // MySQL 使用 LIMIT 子句
                    sql += $" LIMIT {limit}";
                }
                else
                {
                    throw new NotSupportedException($"Database type '{dbType}' is not supported for pagination.");
                }
            }

            return await ExecuteWithTransactionAsync(async (conn, trans) =>
            {
                Console.WriteLine($"Executing SQL: {sql}");
                var result = await conn.QueryAsync<T>(new CommandDefinition(sql, parameters, trans, cancellationToken: cancellationToken));
                return result.ToList();
            });
        }


        private async Task<T> ExecuteWithTransactionAsync<T>(Func<DbConnection, DbTransaction?, Task<T>> action)
        {
            return _transactionManager.Transaction != null
                ? await action(_connection, _transactionManager.Transaction)
                : await action(_connection, null);
        }
        private string GetDatabaseType()
        {
            string dbType = _connection.GetType().Name.ToLower();
            if (dbType.Contains("mysqlconnection"))
                return "MySQL";
            if (dbType.Contains("sqlconnection"))
                return "SQLServer";
           
            throw new NotSupportedException($"Unsupported database connection type: {dbType}");
        }
        private string InsertTopClause(string sql, int limit)
        {
            // 確保 SELECT 子句中包含 TOP
            if (sql.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
            {
                return sql.Insert(6, $" TOP {limit} ");
            }

            throw new ArgumentException("Invalid SQL format for inserting TOP clause.", nameof(sql));
        }
    }
}

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
    /// �q�θ�Ʈw�ާ@���O�C
    /// </summary>
    public class GenericDb : BaseDAL, IGenericDb
    {
        private readonly ITransactionManager _transactionManager;
        private DbConnection _connection;

        /// <summary>
        /// ��l�� GenericDb �����s��ҡC
        /// </summary>
        ///<param name="connection">��Ʈw�s�u����</param>
        ///<param name="transactionManager">����Ҧ�����</param>
        public GenericDb(DbConnection connection ,ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager ?? throw new ArgumentNullException(nameof(transactionManager));
            _connection = connection;
        }

        /// <summary>
        /// ��s��Ʈw�s���H���C
        /// </summary>
        /// <param name="connectionInfo">�s����Ʈw�s���H���C</param>
        /// ���
        public void UpdateConnectionInfo(string connectionString, int dbType)
        {
            throw new NotImplementedException(); 
        }

        /// <summary>
        /// ��s��Ʈw�s���H���C
        /// </summary>
        /// <param name="connectionString">��Ʈw�s�u�r��C</param>
        /// <param name="dbType">��Ʈw�����C</param>
        public void UpdateConnectionInfo(DbConnection newConnection)
        {
            if (newConnection == null)
                throw new ArgumentNullException(nameof(newConnection));

            // ����{���s�u
            //if (Connection != null)
            //{
            //    if (Connection.State == ConnectionState.Open)
            //        Connection.Close();
            //    Connection.Dispose();
            //}
            _connection = newConnection;
            // ��s BaseDAL �����s�u
            //SetConnection(newConnection);            
        }

        /// <summary>
        /// �����Ʈw�s�������C
        /// </summary>
        /// <returns>��Ʈw�s�������C</returns>
        /// ���
        public string GetDbConnectionType()
        {
            return "null";
        }

        /// <summary>
        /// �����Ʈw�s���r��C
        /// </summary>
        /// <returns>��Ʈw�s���r��C</returns>
        /// ���
        public string GetDbConnectionString()
        {
            return _connection.ConnectionString;
        }

        /// <summary>
        /// �����Ʈw�s���C
        /// </summary>
        /// <returns>��Ʈw�s���C</returns>
        /// ���
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

            // �P�_��Ʈw����
            string dbType = GetDatabaseType();

            if (limit > 0)
            {
                if (dbType == "SQLServer")
                {
                    // SQL Server �ϥ� TOP �l�y
                    sql = InsertTopClause(sql, limit);
                }
                else if (dbType == "MySQL")
                {
                    // MySQL �ϥ� LIMIT �l�y
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
            // �T�O SELECT �l�y���]�t TOP
            if (sql.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
            {
                return sql.Insert(6, $" TOP {limit} ");
            }

            throw new ArgumentException("Invalid SQL format for inserting TOP clause.", nameof(sql));
        }
    }
}

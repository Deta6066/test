using Dapper;
using DapperDataBase.Database;
using DapperDataBase.Database.Interface;
using System.Collections.Concurrent;
using System.Data.Common;
using VisLibrary.Models;
using VisLibrary.SqlGenerator;

namespace VisLibrary.Repositories.Interface
{
    /// <summary>
    /// 通用的倉庫介面，定義基本的CRUD操作。
    /// 適用於SQL指令及組合綁定在倉庫的情況
    /// </summary>
    /// <typeparam name="T">實體類型</typeparam>
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByPk(int? pk);
        Task<int> Insert(T obj, bool autoIncrement = true);
        Task<int> Update(T obj);
        Task<int> Delete(int pk);
        Task<List<T>> GetAll();

        Task<int> Insert(string sql, Dictionary<string, object?> prams);
        Task<int> Update(string sql, Dictionary<string, object?> prams);
        Task<int> Delete(string sql, Dictionary<string, object?> prams);
        Task<List<T>> GetBySql(string sql, ConcurrentDictionary<string, object?> prams);
        Task<List<T>> GetBySql(string sql, ConcurrentDictionary<string, object?> prams, CancellationToken token = default);

        void UpdateConnectionInfo(DbConnectionInfo newConnectionInfo);
        void UseVisConnection();
        string GetDbConn();
    }
    public interface IGenericRepositoryBase<T> where T : class
    {
        Task<T?> GetByPk(int? pk,string tableName);
        Task<int> Insert(T obj, string tableName, bool autoIncrement = true);
        Task<int> Update(T obj, string tableName);
        Task<int> Delete(int pk, string tableName);
        Task<List<T>> GetAll(string tableName);
        Task<List<T>> GetBySearch(T searchObj, string tableName);
        Task<int> Insert(string sql, DynamicParameters? parameters);
        Task<int> Update(string sql, DynamicParameters? parameters);
        Task<int> Delete(string sql, DynamicParameters? parameters);
        Task<List<T>> GetBySql(string sql, DynamicParameters? parameters, int limit = 0, CancellationToken cancellationToken = default);

        void UpdateConnectionInfo(string connectionString, int dbType);
        void UseVisConnection();
        string GetDbConn();
        //DynamicParameters? GenerateParameters(T obj, OperationType operationType);

    }
    /// <summary>
    /// 定義更新資料庫連接信息的方法。
    /// </summary>
    public interface IDbConnectionManager
    {
        void UpdateConnectionInfo(DbConnectionInfo newConnectionInfo);
        void UseVisConnection();
        IGenericDb GetDb();
        string GetDbConn();
        DbConnection GetConnection();
        public string GetDbConnectionType();
    }

    public interface IPropertyProcessor
    {
        void TrimStringProperties<T>(T instance);
        void FormatDateTimeProperties<T>(T instance);
        void ProcessProperties<T>(T instance);
        List<T> ProcessPropertiesForList<T>(List<T> list);
    }
}
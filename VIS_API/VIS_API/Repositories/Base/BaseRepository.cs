using Dapper;
using DapperDataBase.Database.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using VIS_API.Models;
using VIS_API.Repositories.Interface;
using VIS_API.SqlGenerator;
using VIS_API.Utilities;

namespace VIS_API.Repositories.Base
{
    

    public class GenericRepositoryBase<T> : IGenericRepositoryBase<T> where T : class
    {

        protected readonly IPropertyProcessor _propertyProcessor;
        protected readonly IGenericDb _db;
        protected readonly ISqlGenerator<T> _sqlGenerator;


        /// <summary>
        /// 初始化 BaseRepository 類的新實例。
        /// </summary>
        /// <param name="dbFactory">資料庫工廠。</param>
        public GenericRepositoryBase(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<T> sqlGenerator)
        {
            _propertyProcessor = propertyProcessor ?? throw new ArgumentNullException(nameof(propertyProcessor));
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _sqlGenerator = sqlGenerator ?? throw new ArgumentNullException(nameof(sqlGenerator));

        }

        /// <summary>
        /// 更新資料庫連接信息。
        /// </summary>
        /// <param name="connectionInfo">新的資料庫連接信息。</param>
        public virtual void UpdateConnectionInfo(string connectionString, int dbType)
        {
            _db.UpdateConnectionInfo(connectionString, dbType);
        }

        public void UseVisConnection()
        {
            //_db.UseVisConnection();
        }


        public virtual async Task<int> Insert(string sql, DynamicParameters? parameters)
        {

            return await _db.ExecuteNonQueryAsync(sql, parameters);
        }
        public virtual async Task<int> Update(string sql, DynamicParameters? parameters)
        {
            return await _db.ExecuteNonQueryAsync(sql, parameters);
        }

        public virtual async Task<int> Delete(string sql, DynamicParameters? parameters)
        {
            return await _db.ExecuteNonQueryAsync(sql, parameters);
        }
        public virtual async Task<List<T>> GetBySql(string sql, DynamicParameters? parameters, int limit = 0, CancellationToken cancellationToken = default)
        {
            try
            {
                List<T> result = new List<T>();
                                
                result = await _db.GetListAsync<T>(sql, parameters, limit, cancellationToken);

                var resultList = result.ToList();
                _propertyProcessor.ProcessPropertiesForList(resultList);
                // 去除列表中每个对象的字符串属性的前后空白
                return resultList;
            }
            catch (TimeoutException e)
            {
                // 保留原始異常資訊並傳遞錯誤
                throw new Exception($"GetBySql Timeout Error: {e.Message}", e);
            }
            catch (Exception e)
            {
                // 捕捉所有其他異常，並保留堆疊訊息
                throw new Exception($"GetBySql General Error: {e.Message}", e);
            }
        }

        public virtual string GetDbConn()
        {
            return "";
        }

        
        /// <summary>
        /// 插入資料。
        /// </summary>
        public async Task<int> Insert(T obj, string tableName, bool autoIncrement = true)
        {
            _propertyProcessor.ProcessProperties(obj);
            string insertQuery = _sqlGenerator.GenerateInsertQuery(tableName, obj, autoIncrement);
            DynamicParameters parameters = _sqlGenerator.GenerateParameters(obj, OperationType.Insert);

            
            return await _db.ExecuteNonQueryAsync(insertQuery, parameters);
          
        }

        /// <summary>
        /// 更新資料。
        /// </summary>
        public async Task<int> Update(T obj, string tableName)
        {
            _propertyProcessor.ProcessProperties(obj);
            string updateQuery = _sqlGenerator.GenerateUpdateQuery(tableName, obj);
            DynamicParameters parameters = _sqlGenerator.GenerateParameters(obj, OperationType.Update);
            return await _db.ExecuteNonQueryAsync(updateQuery, parameters);
        }

        /// <summary>
        /// 刪除資料。
        /// </summary>
        public async Task<int> Delete(int pk, string tableName)
        {
            // 需要獲取主鍵屬性名稱
            //string primaryKey = _sqlGenerator.GetPrimaryKey();
            string deleteQuery = $"DELETE FROM {tableName} WHERE company_fk = @pk";
            var parameters = new DynamicParameters();
            parameters.Add("pk", pk);

            return await _db.ExecuteNonQueryAsync(deleteQuery, parameters);
        }

        /// <summary>
        /// 獲取所有資料。
        /// </summary>
        public async Task<List<T>> GetAll(string tableName)
        {
            string selectQuery = $"SELECT * FROM {tableName}";
            var result = await _db.GetListAsync<T>(selectQuery, new DynamicParameters());
            _propertyProcessor.ProcessPropertiesForList(result.ToList());
            return result.ToList();
        }

        /// <summary>
        /// 根據主鍵獲取資料。
        /// </summary>
        public virtual async Task<T?> GetByPk(int? pk, string tableName)
        {
            //string primaryKey = _sqlGenerator.GetPrimaryKey();
            string selectQuery = $"SELECT * FROM {tableName} WHERE company_fk = @pk";
            var parameters = new DynamicParameters();
            parameters.Add("pk", pk);

            var result = await _db.GetListAsync<T>(selectQuery, parameters);
            return result.SingleOrDefault();
        }

        /// <summary>
        /// 根據 SearchKey 獲取資料。
        /// </summary>
        public async Task<List<T>> GetBySearch(T searchObj, string tableName)
        {
            string selectQuery = _sqlGenerator.GenerateSelectQuery(tableName, searchObj, out DynamicParameters parameters);
            var result = await _db.GetListAsync<T>(selectQuery, parameters);
            _propertyProcessor.ProcessPropertiesForList(result.ToList());
            return result.ToList();
        }

        public DynamicParameters? GenerateParameters(T obj, OperationType operationType)
        {
            return _sqlGenerator.GenerateParameters(obj, operationType);
        }
    }
}

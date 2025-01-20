using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Repositories.Base;

namespace VIS_API.SqlGenerator
{
    public class SqlGenerator<T> : ISqlGenerator<T> where T : class
    {
        protected virtual string TableName => typeof(T).Name;

        public string GenerateInsertQuery(string tableName, T obj, bool autoIncrement)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                      .Where(p => !IsPrimaryKey(p))
                                      .Where(p => !Attribute.IsDefined(p, typeof(InsertIgnoreAttribute)))
                                      .ToList();

            var columns = string.Join(", ", properties.Select(p => p.Name));
            var parameters = string.Join(", ", properties.Select(p => "@" + p.Name));

            string sql = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

            if (autoIncrement)
            {
                // SCOPE_IDENTITY()為sql server專用，mysql為LAST_INSERT_ID()
                //先註解
                // sql += "; SELECT CAST(SCOPE_IDENTITY() as int)";
            }

            return sql;
        }

        public string GenerateUpdateQuery(string tableName, T obj)
        {
            string primaryKey = GetPrimaryKey();

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                      .Where(p => p.Name != primaryKey)
                                      .Where(p => !Attribute.IsDefined(p, typeof(UpdateIgnoreAttribute)))
                                      .ToList();

            var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
            string whereClause = GenerateWhereClause(obj);

            string sql = $"UPDATE {tableName} SET {setClause} {whereClause}";
            return sql;
        }

        public string GenerateDeleteQuery(string tableName, T obj)
        {
            var whereClause = GenerateWhereClause(obj);
            string sql = $"DELETE FROM {tableName} {whereClause}";
            return sql;
        }

        public string GenerateSelectQuery(string tableName, T obj, out DynamicParameters parameters)
        {
            var selectColumns = "*"; // 或根據需求指定具體列

            var whereClause = GenerateWhereClause(obj, out parameters);

            string sql = $"SELECT {selectColumns} FROM {tableName} {whereClause}";
            return sql;
        }

        public DynamicParameters GenerateParameters(T obj, OperationType operationType)
        {
            string primaryKey = GetPrimaryKey();
            var parameters = new DynamicParameters();

            foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                bool isInsertIgnore = Attribute.IsDefined(prop, typeof(InsertIgnoreAttribute));
                bool isUpdateIgnore = Attribute.IsDefined(prop, typeof(UpdateIgnoreAttribute));
                bool isSearchKey = Attribute.IsDefined(prop, typeof(SearchKeyAttribute));

                if ((operationType == OperationType.Insert && (isInsertIgnore || isSearchKey)) ||
                    (operationType == OperationType.Update && isUpdateIgnore))
                {
                    // 排除被標記的屬性
                    continue;
                }

                string key = prop.Name;
                object? value = prop.GetValue(obj) ?? DBNull.Value;
                parameters.Add(key, value);
            }

            // 如果是 Delete 或 Select，包含 SearchKey 屬性
            if (operationType == OperationType.Delete || operationType == OperationType.Select)
            {
                foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    bool isSearchKey = Attribute.IsDefined(prop, typeof(SearchKeyAttribute));
                    if (isSearchKey)
                    {
                        string key = prop.Name;
                        object? value = prop.GetValue(obj) ?? DBNull.Value;
                        parameters.Add(key, value);
                    }
                }
            }

            return parameters;
        }

        /// <summary>
        /// 獲取被 [PrimaryKey] 標記的屬性名稱。
        /// </summary>
        /// <returns>主鍵名稱。</returns>
        private string GetPrimaryKey()
        {
            var primaryKeyProperty = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                             .FirstOrDefault(p => Attribute.IsDefined(p, typeof(PrimaryKeyAttribute)));
            if (primaryKeyProperty != null)
            {
                return primaryKeyProperty.Name;
            }

            // 如果沒有標記，則根據命名約定（如 "Id" 或 "ClassNameId"）
            var conventionPrimaryKey = "Id";
            if (typeof(T).GetProperty(conventionPrimaryKey) != null)
            {
                return conventionPrimaryKey;
            }

            // 可以根據需要添加更多的命名約定
            throw new InvalidOperationException($"No primary key defined for type {typeof(T).Name}");
        }

        /// <summary>
        /// 判斷屬性是否為主鍵。
        /// </summary>
        /// <param name="property">屬性資訊。</param>
        /// <returns>是否為主鍵。</returns>
        private bool IsPrimaryKey(PropertyInfo property)
        {
            return Attribute.IsDefined(property, typeof(PrimaryKeyAttribute));
        }

        /// <summary>
        /// 根據標記的 [SearchKey] 屬性生成 WHERE 子句。
        /// </summary>
        /// <param name="obj">物件實例。</param>
        /// <returns>WHERE 子句。</returns>
        private string GenerateWhereClause(T obj)
        {
            var searchKeys = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                     .Where(p => Attribute.IsDefined(p, typeof(SearchKeyAttribute)))
                                     .ToList();

            if (!searchKeys.Any())
                return string.Empty;

            var conditions = searchKeys.Select(p => $"{p.Name} = @{p.Name}");
            string whereClause = "WHERE " + string.Join(" AND ", conditions);
            return whereClause;
        }

        /// <summary>
        /// 根據標記的 [SearchKey] 屬性生成 WHERE 子句並填充參數。
        /// </summary>
        /// <param name="obj">物件實例。</param>
        /// <param name="parameters">輸出參數。</param>
        /// <returns>WHERE 子句。</returns>
        private string GenerateWhereClause(T obj, out DynamicParameters parameters)
        {
            parameters = new DynamicParameters();

            var searchKeys = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                     .Where(p => Attribute.IsDefined(p, typeof(SearchKeyAttribute)))
                                     .ToList();

            if (!searchKeys.Any())
                return string.Empty;

            var conditions = new List<string>();
            foreach (var prop in searchKeys)
            {
                conditions.Add($"{prop.Name} = @{prop.Name}");
                object? value = prop.GetValue(obj) ?? DBNull.Value;
                parameters.Add(prop.Name, value);
            }

            string whereClause = "WHERE " + string.Join(" AND ", conditions);
            return whereClause;
        }
    }
}

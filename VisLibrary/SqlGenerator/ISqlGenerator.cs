using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.SqlGenerator
{
    public interface ISqlGenerator<T> where T : class
    {
        string GenerateInsertQuery(string tableName, T obj, bool autoIncrement);
        string GenerateUpdateQuery(string tableName, T obj);
        string GenerateDeleteQuery(string tableName, T obj);
        string GenerateSelectQuery(string tableName, T obj, out DynamicParameters parameters);
        DynamicParameters GenerateParameters(T obj, OperationType operationType);
    }

    public enum OperationType
    {
        Insert,
        Update,
        Delete,
        Select
    }
}

using System.Collections.Concurrent;
using System.Data.Common;
using Dapper;
namespace DapperDataBase.Database.Interface
{
    /// <summary>
    /// 定義資料庫操作的介面。
    /// </summary>
    public interface IDb
    {
        /// <summary>
        /// 執行查詢並返回單一值。
        /// </summary>
        /// <typeparam name="T">查詢結果的類型。</typeparam>
        /// <param name="sql">SQL查詢語句。</param>
        /// <param name="parameters">查詢參數。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        /// <returns>查詢結果。</returns>
        Task<T?> ExecuteScalarAsync<T>(string sql, DynamicParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 執行非查詢操作（如插入、更新、刪除）。
        /// </summary>
        /// <param name="sql">SQL操作語句。</param>
        /// <param name="parameters">操作參數。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        /// <returns>受影響的行數。</returns>
        Task<int> ExecuteNonQueryAsync(string sql, DynamicParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 獲取查詢結果列表。
        /// </summary>
        /// <typeparam name="T">查詢結果的類型。</typeparam>
        /// <param name="sql">SQL查詢語句。</param>
        /// <param name="parameters">查詢參數。</param>
        /// <param name="limit">查詢結果的限制。0為無限制。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        /// <returns>查詢結果列表。</returns>
        Task<List<T>> GetListAsync<T>(string sql, DynamicParameters? parameters = null, int limit = 0, CancellationToken cancellationToken = default) where T : class;

    }
}
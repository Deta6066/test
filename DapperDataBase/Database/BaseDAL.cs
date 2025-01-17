using System.Data.Common;

namespace DapperDataBase.Database
{
    /// <summary>
    /// 基礎資料庫訪問層。
    /// </summary>
    public class BaseDAL
    {
        /// <summary>
        /// 数据库连接。
        /// </summary>
        //protected DbConnection Connection { get; private set; }

        /// <summary>
        /// 初始化 BaseDAL 类的新实例。
        /// </summary>
        /// <param name="connection">已打开的数据库连接。</param>
        public BaseDAL()
        {
            //Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            // 假设连接已经打开，不再负责打开连接
        }

        /// <summary>
        /// 执行数据库操作。
        /// </summary>
        /// <typeparam name="T">返回类型。</typeparam>
        /// <param name="func">执行操作的函数。</param>
        /// <param name="transaction">数据库事务。</param>
        /// <returns>操作结果。</returns>
        protected async Task<T> ExecuteAsync<T>(Func<DbConnection, DbTransaction, Task<T>> func, DbConnection connection, DbTransaction transaction)
        {
            return await func(connection, transaction);
        }

        /// <summary>
        /// 执行数据库操作，带有 CancellationToken。
        /// </summary>
        protected async Task<T> ExecuteAsync<T>(Func<DbConnection, DbTransaction, CancellationToken, Task<T>> func, DbConnection connection, DbTransaction transaction , CancellationToken cancellationToken = default)
        {
            return await func(connection, transaction, cancellationToken);
        }

        protected async Task<T> ExecuteAsync<T>(Func<DbConnection, Task<T>> func, DbConnection connection)
        {
            return await func(connection);
        }
    }

}
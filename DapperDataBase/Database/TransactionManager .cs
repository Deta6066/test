using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperDataBase.Database
{
    public interface ITransactionManager : IAsyncDisposable
    {
        DbConnection? Connection { get; }
        DbTransaction? Transaction { get; }
        Task InitializeAsync(DbConnection connection);
        Task OpenAsyncConnection(DbConnection connection);

        Task CommitAsync();
        Task RollbackAsync();
        void SetTransaction(DbTransaction? transaction);
        void UpdateConnection(DbConnection newConnection); // 新增此方法

    }

    public class MyTransactionManager : ITransactionManager
    {
        public DbConnection? Connection { get; private set; }
        public DbTransaction? Transaction { get; private set; }
        private bool _isDisposed = false;

        

        public async Task InitializeAsync(DbConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));

            if (Connection.State != ConnectionState.Open)
            {
                await Connection.OpenAsync();
                Transaction = await Connection.BeginTransactionAsync();

            }

        }
        public async Task OpenAsyncConnection(DbConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));

            if (Connection.State != ConnectionState.Open)
            {
                await Connection.OpenAsync();
            }
        }

        public async Task CommitAsync()
        {
            if (!_isDisposed && Transaction != null)
            {
                await Transaction.CommitAsync();
                //await DisposeAsync();
            }
        }

        public async Task RollbackAsync()
        {
            if (!_isDisposed && Transaction != null)
            {
                await Transaction.RollbackAsync();
                //await DisposeAsync();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (!_isDisposed)
            {
                if (Transaction != null)
                {
                    await Transaction.DisposeAsync();
                    Transaction = null;
                }

                if (Connection != null && Connection.State == ConnectionState.Open)
                {
                    await Connection.CloseAsync();
                    await Connection.DisposeAsync();
                }

                _isDisposed = true;
            }
        }
        public void SetTransaction(DbTransaction? transaction)
        {
            Transaction = transaction;
        }
        public void UpdateConnection(DbConnection newConnection)
        {
            //if (!_isDisposed)
            //    throw new ObjectDisposedException(nameof(MyTransactionManager));

            Connection = newConnection ?? throw new ArgumentNullException(nameof(newConnection));
        }
    }
}

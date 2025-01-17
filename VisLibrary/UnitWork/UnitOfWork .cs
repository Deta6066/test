using DapperDataBase.Database;
using DapperDataBase.Database.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models;
using VisLibrary.Repositories.Interface;
using VisLibrary.Service.Interface;

namespace VisLibrary.UnitWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ITransactionManager _transactionManager;
        private readonly IGenericDb _genericDb;
        private DbConnection _connection;
        private readonly ILogger<UnitOfWork> _logger;

        // 定义具体的仓储接口
        public IRCompanyDataSource RCompanyDataSource { get; }
        private readonly IServiceProvider _serviceProvider;
       public IRSupplierMaterial _rSupplierMaterial { get; }
       public IRHistoricalMaterialUsage RHistoricalMaterialUsage { get; }
        public IRFinishedGoodsInventory RFinishedGoodsInventory { get; }
        public IRScrapQuantity RScrapQuantity { get; }
        public IRMenu _rMenu { get; }
        public IRRole _rRole { get; }

        public IRAssembleCenter _assembleCenter { get; }


        public UnitOfWork(
            ILogger<UnitOfWork> logger,
            ITransactionManager transactionManager,
            IRCompanyDataSource rCompanyDataSource,
            IGenericDb genericDb,
            DbConnection connection,
            IServiceProvider serviceProvider,
            IRSupplierMaterial rSupplierMaterial,
            IRHistoricalMaterialUsage rHistoricalMaterialUsage,
            IRFinishedGoodsInventory rFinishedGoodsInventory,
            IRScrapQuantity rScrapQuantity,
            IRMenu rMenu,
            IRRole rRole,
            IRAssembleCenter assembleCenter
            

        )
        {
            _logger = logger;
            _transactionManager = transactionManager ?? throw new ArgumentNullException(nameof(transactionManager));
            RCompanyDataSource = rCompanyDataSource ?? throw new ArgumentNullException(nameof(rCompanyDataSource));
            _genericDb = genericDb ?? throw new ArgumentNullException(nameof(genericDb));
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _serviceProvider = serviceProvider;
            _rSupplierMaterial = rSupplierMaterial ?? throw new ArgumentNullException(nameof(rSupplierMaterial));
            RHistoricalMaterialUsage = rHistoricalMaterialUsage ?? throw new ArgumentNullException(nameof(rHistoricalMaterialUsage));
            RFinishedGoodsInventory = rFinishedGoodsInventory ?? throw new ArgumentNullException(nameof(rFinishedGoodsInventory));
            RScrapQuantity = rScrapQuantity ?? throw new ArgumentNullException(nameof(rScrapQuantity));
            _rMenu = rMenu ?? throw new ArgumentNullException(nameof(rMenu));
            _rRole = rRole ?? throw new ArgumentNullException(nameof(rRole));
            _assembleCenter = assembleCenter;
            // 初始化其他仓储接口
            // ROrder = rOrder ?? throw new ArgumentNullException(nameof(rOrder));
            // RInventory = rInventory ?? throw new ArgumentNullException(nameof(rInventory));
        }

        /// <summary>
        /// 初始化連線資訊與建立新的交易連線。
        /// </summary>
        public async Task InitializeAsync()
        {
            try
            {
                await _transactionManager.InitializeAsync(_connection);
                //_genericDb.SetTransaction(_transactionManager.Transaction);
            }catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing InitializeAsync.");
                throw;   
            }

        }
        public async Task OpenAsyncConnection()
        {
            try
            {
                await _transactionManager.OpenAsyncConnection(_connection);
                //_genericDb.SetTransaction(_transactionManager.Transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing OpenAsyncConnection.");
                throw;
            }

        }

        public async Task CommitAsync()
        {
            await _transactionManager.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _transactionManager.RollbackAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _transactionManager.DisposeAsync();
            // 如果有其他需要释放的资源，可以在这里释放
            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
                await _connection.DisposeAsync();
            }
        }
        public IGenericRepositoryBase<T> GetRepository<T>() where T : class
        {
            // 使用服务提供者创建 GenericRepository<T> 实例
            return _serviceProvider.GetRequiredService<IGenericRepositoryBase<T>>();
        }

        /// <summary>
        /// 更新連線資訊並建立新的交易連線。
        /// </summary>
        /// <param name="connectionString">新的連線資訊。</param>
        /// <param name="dbType">資料庫類型。</param>
        public void UpdateConnectionInfo(string? connectionString, int dbType)
        {
            if (_transactionManager.Transaction != null)
            {
                throw new InvalidOperationException("Cannot update connection while a transaction is active. Please commit or rollback the transaction first.");
            }
            if (connectionString == null)
            {
                throw new InvalidOperationException(nameof(connectionString));
            }
            // 釋放現有連線
            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
                //_connection.Dispose();
            }

            // 創建新的連線
            var dbFactory = _serviceProvider.GetRequiredService<IDbFactory>();
            var newConnection = dbFactory.CreateConnection(connectionString,dbType);

            // 更新 _connection 字段
            _connection = newConnection;

            // 更新 GenericDb 使用的新連線
            _genericDb.UpdateConnectionInfo(_connection);

            // 更新 TransactionManager 使用的新連線
            _transactionManager.UpdateConnection(_connection);
        }
    }
}

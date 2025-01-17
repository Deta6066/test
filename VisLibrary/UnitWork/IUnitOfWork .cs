using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models;
using VisLibrary.Repositories.Interface;
using VisLibrary.Service.Interface;

namespace VisLibrary.UnitWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        // 定義具體的倉庫接口屬性
        IGenericRepositoryBase<T> GetRepository<T>() where T : class;

        IRCompanyDataSource RCompanyDataSource { get; }
        // 添加其他需要的倉庫接口，例如：
        // IROrder ROrder { get; }
        // IRInventory RInventory { get; }
        public IRSupplierMaterial _rSupplierMaterial { get; }

        public IRHistoricalMaterialUsage RHistoricalMaterialUsage { get; }
        public IRFinishedGoodsInventory RFinishedGoodsInventory { get; }
        public IRScrapQuantity RScrapQuantity { get; }
        public IRMenu _rMenu { get; }
        public IRRole _rRole { get; }
        public IRAssembleCenter _assembleCenter { get; }

        // 事務管理方法
        Task InitializeAsync();
        Task OpenAsyncConnection();
        Task CommitAsync();
        Task RollbackAsync();
        public void UpdateConnectionInfo(string? ConnectionString, int dbType);
    }
}

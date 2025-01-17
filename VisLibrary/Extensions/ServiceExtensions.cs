using VisLibrary.Business.Interface;
using VisLibrary.Models;
using VisLibrary.Repositories;
using VisLibrary.Models.WHE;
using VisLibrary.Repositories.Interface;
using VisLibrary.Service.Interface;
using VisLibrary.Service;
using Microsoft.Extensions.DependencyInjection;
using VisLibrary.Business;
using Service.Interface;
using Repositories;
using VisLibrary.Utilities;
using VisLibrary.Service.Base;
using VisLibrary.Repositories.Base;
using Microsoft.Extensions.Configuration;
using VisLibrary.Repositories.CNC;
using VisLibrary.Service.PMD;
using DapperDataBase.Database;
using System.Data.Common;
using VisLibrary.UnitWork;
using DapperDataBase.Database.Interface;
using VisLibrary.SqlGenerator;
using NLog.Config;
using System.Transactions;
using System.Configuration;
using System.Data;
using VisLibrary.Service.CNC;
using VisLibrary.Service.JWT;

namespace VisLibrary.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped<IRRole, RRole>();
            services.AddScoped<IRMenu, RMenu>();

            services.AddScoped<IRCompanyDataSource, RCompanyDataSource>();
            services.AddScoped<IROrder, ROrder>();
            services.AddScoped<IRInventory, RInventory>();
            services.AddScoped<IRSupplierMaterial, RSupplierMaterial>();
            services.AddScoped<IRHistoricalMaterialUsage, RHistoricalMaterialUsage>();
           
            services.AddScoped<IRFinishedGoodsInventory, RFinishedGoodsInventory>();
            services.AddScoped<IRSupplierScore, RSupplierScore>();

            services.AddScoped<IRAssembleCenter, RAssembleCenter>();


            services.AddScoped<IRStagnantMaterial, RStagnantMaterial>();
            services.AddScoped<IRScrapQuantity, RScrapQuantity>();
            services.AddScoped<IErrorLogRepository, RErrorLog>();
            services.AddScoped<IRMachineInfo, RMachineInfo>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();


            return services;
        }
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISMenu, SMenu>();
            services.AddScoped<ISRole, SRole>();


            services.AddScoped<IBMenu, BMenu>();
            services.AddScoped<ISUser, SUser>();
            services.AddScoped<ISCompanyDataSource, SCompanyDataSource>();
            services.AddScoped<ISCompany, SCompany>();
            services.AddScoped<ISInventory, SInventoryQuantity>();
            services.AddScoped   <IOrder, SOrder>();
            services.AddScoped<ISSupplierMaterial, SSupplierMaterial>();
            services.AddScoped<ISHistoricalMaterialUsage, SHistoricalMaterialUsage>();
            services.AddScoped<ISGeneric, SGeneric>();
            services.AddScoped<ISFinishedGoodsInventory, SFinishedGoodsInventory>();
            services.AddScoped<ISSupplierDeliveryRate, SSupplierDeliveryRate>();

          //  services.AddScoped<ISStagnantMaterial, SStagnantMaterial>();
            services.AddScoped<IScrapQuantity, SScrapQuantity>();
            services.AddScoped<ISMachineInfo , SMachineInfo>();
            services.AddScoped<ISAps_info, SAps_info>();
            services.AddScoped<ISStatus_currently_info, SStatus_currently_info>();
            services.AddScoped<ISShipment, SShipment>();
            services.AddScoped< ISInactiveCustomer ,SInactiveCustomer >();
            services.AddScoped<ISProductionShiftChart, SProductionShiftChart>();
            services.AddScoped<IGenericDb, GenericDb>();
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

            services.AddSingleton<IJwtTokenService, JwtTokenService>();

            services.AddScoped<ISessionService, SessionServiceDapper>();


            return services;
        }
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IPropertyProcessor, PropertyProcessorBase>();
            services.AddSingleton<ExceptionHandler>();
            services.AddScoped<ITransactionManager, MyTransactionManager>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(ISqlGenerator<>), typeof(SqlGenerator<>));
            services.AddScoped(typeof(IGenericRepositoryBase<>), typeof(GenericRepositoryBase<>));
            //使用工廠模式手動獲取初始連線
            services.AddScoped<DbConnection>(provider =>
            {
                var dbFactory = provider.GetRequiredService<IDbFactory>();

                return dbFactory.CreateConnection("mysql");
            });

            services.AddScoped<ISStagnantMaterial, SStagnantMaterial>();
            services.AddScoped<IScrapQuantity, SScrapQuantity>();
            services.AddScoped<ISMachineInfo , SMachineInfo>();
            services.AddScoped<ISAps_info, SAps_info>();
            services.AddScoped<ISStatus_currently_info, SStatus_currently_info>();
            services.AddScoped<ISShipment, SShipment>();
            services.AddScoped< ISInactiveCustomer ,SInactiveCustomer >();
            services.AddScoped<ISProductionShiftChart, SProductionShiftChart>();
            services.AddScoped<ISProductLine, SProductLine>();
            services.AddScoped< ISUnreturnedTransportRack , SUnreturnedTransportRack>();
            services.AddScoped<IAssembleCenter, SAssembleCenter>();
            services.AddTransient<ISProductionHistory, SProductionHistory>();
            services.AddScoped<ISOperationalRate, SOperationalRate>();
            services.AddScoped< ISMachineGroup , SMachineGroup>();
            services.AddScoped<ISStatus_history_info, SStatus_history_info>();
            services.AddScoped< ISMachineImage , SMachineImage>();
            services.AddScoped<ISMachineGroup, SMachineGroup>();
            services.AddScoped<ISnomalyHistory, SNomalyHistory>();
            return services;
        }
            /// <summary>
            /// 添加自定義服務配置，包括控制器、分佈式內存緩存、數據保護和會話配置。
            /// </summary>
            /// <param name="services">服務集合。</param>
            /// <param name="configuration">配置對象。</param>
            public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddDistributedMemoryCache();
            services.AddDataProtection();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(2);
            });

            services.Configure<SqlCmdConfig>(configuration.GetSection("SqlCmd"));
            services.AddSingleton<IDbFactory, DbFactory>();

            services.AddRepositoryServices();
            services.AddBusinessServices(configuration);
            services.AddInfrastructureServices();
            services.AddHttpContextAccessor();
            services.AddScoped<TokenService>();
        }
    }
}

using VIS_API.Models;
using VIS_API.Models.ViewModel;
using VIS_API.Repositories.Interface;
using VIS_API.Service.Interface;
using VIS_API.Models.API;
using Newtonsoft.Json;
using VIS_API.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VIS_API.Service.Base;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using VIS_API.Models.View;
using DapperDataBase.Database;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using VIS_API.Repositories;
using VIS_API.UnitWork;
using System.ComponentModel.Design;
using Microsoft.IdentityModel.Tokens;
using Utility = VIS_API.Utilities.Utility;
using Dapper;

namespace VIS_API.Service
{
    public class SOrder : ServiceBase, IOrder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ExceptionHandler _exceptionHandler;
        private readonly ILogger<SOrder> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public SOrder(ExceptionHandler exceptionHandler, IServiceProvider serviceProvider, ILogger<SOrder> logger, IOptions<SqlCmdConfig> sqlCmdConfig, IUnitOfWork unitOfWork)
            : base(sqlCmdConfig)
        {
            _serviceProvider = serviceProvider;
            _exceptionHandler = exceptionHandler;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 獲取訂單列表和逾期訂單列表的綜合信息。
        /// </summary>
        /// <param name="parameter">訂單查詢參數。</param>
        /// <returns>包含訂單列表和逾期訂單列表的 VOrder 物件。</returns>
        public async Task<VOrder?> GetList(OrderParameter parameter)
        {
            try
            {
                await _unitOfWork.OpenAsyncConnection();

                // 獲取裝配中心、公司數據源和生產線組列表
                var companyDataSource = await GetCompanyDataSource(_unitOfWork,parameter);
                if (companyDataSource == null) return null; 

                var assemblecenterList = await GetAssembleCenter(_unitOfWork,parameter.Company_fk);
                var productionLineGroupList = await GetProductionGroup(_unitOfWork,parameter.assemblecenter_fk);
                //await _unitOfWork.CommitAsync();


                _unitOfWork.UpdateConnectionInfo(companyDataSource.dbParamater, companyDataSource.dbType);
                await _unitOfWork.InitializeAsync();

                // 並行獲取訂單列表和逾期訂單列表
                var orderList = await GetOrderList(_unitOfWork,companyDataSource, parameter);

                switch (parameter.OrderStatus)
                {
                    case 1:
                        orderList = orderList?.Where(x => x.OrderStatusType == 1).ToList();
                        break;
                    case 2:
                        orderList = orderList?.Where(x => x.OrderStatusType == 2).ToList();
                        break;
                    default:
                        break;
                }
                var orderOverdueList = await GetOrderOverdueList(_unitOfWork,companyDataSource, parameter);
                //await Task.WhenAll(orderListTask, orderOverdueListTask);
                //var orderList = await orderListTask;
                //var orderOverdueList = await orderOverdueListTask;
                //var json = JsonConvert.SerializeObject(productionLineGroupList);

                var uniqueSNames = productionLineGroupList
                            .GroupBy(item => item.sName)
                            .Select(group => group.First())
                            .ToList();
                List<string> sNames = [];
                foreach (var sName in uniqueSNames)
                {
                    if (!string.IsNullOrEmpty(sName.sName))
                        sNames.Add(sName.sName);
                }
                // 更新生產線組信息
                orderList=orderList == null? orderList : UpdateProductLineGroupWithDictionary(orderList, productionLineGroupList);
                orderOverdueList= orderOverdueList==null? orderOverdueList:UpdateProductLineGroupWithDictionary(orderOverdueList, productionLineGroupList);

                // 構建 VOrder 物件
                var vOrder = new VOrder
                {
                    OrderList = orderList,
                    OrderOverdue = orderOverdueList,
                    Assemblecenter = assemblecenterList,
                    assembleGroup = sNames
                };
                await _unitOfWork.CommitAsync();
                return vOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetList failed.");
                return new VOrder();
            }
            finally
            {
                await _unitOfWork.DisposeAsync();
            }

        }

        
        /// <summary>
        /// 根據參數獲取訂單列表的實際邏輯。
        /// </summary>
        /// <param name="companyDataSource">公司數據源。</param>
        /// <param name="parameter">訂單查詢參數。</param>
        /// <returns>訂單列表。</returns>
        private async Task<List<Order>?> FetchOrderListAsync(IUnitOfWork unitOfWork, MCompanyDataSource companyDataSource, OrderParameter? parameter)
        {
            if (parameter == null)
            {
                return null;
            }


            var orders = unitOfWork.GetRepository<Order>();
            var sqlInfo = Utility.FromJson<SqlInfo>(companyDataSource.sqlcmd);
            var keyValuePairs = Utility.CreateDynamicParameters(parameter);
            if(sqlInfo==null) return null;
            string sql = $@"SELECT * FROM ( SELECT item_22.pline_no AS ProductLineNo, cust.custnm2 AS CustomerName, mkordsub.item_no AS ItemNumber, mkordsub.lot_no AS LotNumber, cordsub.sclose AS OrderStatus,CASE WHEN cordsub.sclose='結案' THEN  1 WHEN cordsub.sclose='未結' THEN 2 END as OrderStatusType, employ.NAME AS Salesman, CONVERT(DATE, cordsub.d_date) AS EstimatedDeliveryDate, CONVERT(DATE, a22_fab.str_date) AS ExpectedStartDate, ( SELECT MAX(CONVERT(DATE, itemio.trn_date)) FROM itemios LEFT JOIN itemio ON itemios.io = itemio.io AND itemios.trn_no = itemio.trn_no WHERE itemios.io = '2' AND itemios.mkord_no = mkordsub.trn_no AND itemios.mkord_sn = mkordsub.sn AND itemio.s_desc <> '歸還' AND itemio.mk_type = '成品入庫' ) AS ReceivedDate, CONVERT(DATE, invosub.trn_date) AS ShipmentDate, CONVERT(VARCHAR(12), CONVERT(MONEY, cordsub.quantity), 1) AS Quantity, cordsub.amount AS Amount, ( CASE WHEN SUBSTRING(cordsub.d_date, 7, 2) <= 25 THEN SUBSTRING(cordsub.d_date, 1, 6) WHEN SUBSTRING(cordsub.d_date, 7, 2) >= 25 THEN CASE WHEN SUBSTRING(cordsub.d_date, 5, 2) = 12 THEN CAST(SUBSTRING(cordsub.d_date, 1, 4) + 1 AS VARCHAR) + '01' ELSE SUBSTRING(cordsub.d_date, 1, 6) + 1 END END ) AS OrderMonth FROM cordsub LEFT JOIN a22_fab ON cordsub.trn_no = a22_fab.cord_no AND cordsub.sn = a22_fab.cord_sn LEFT JOIN mkordsub ON cordsub.trn_no = mkordsub.cord_no AND cordsub.sn = mkordsub.cord_sn LEFT JOIN item_22 ON item_22.item_no = cordsub.item_no LEFT JOIN cord ON cord.trn_no = cordsub.trn_no LEFT JOIN cust ON cust.cust_no = CASE WHEN LEN(cordsub.cust_no) = 0 THEN cord.cust_no ELSE cordsub.cust_no END LEFT JOIN employ ON employ.emp_no = cord.user_code LEFT JOIN invosub ON cordsub.trn_no = invosub.cord_no AND cordsub.sn = invosub.cord_sn AND invosub.lot_no = mkordsub.lot_no ) AS MainQuery WHERE 1 <= MainQuery.ProductLineNo AND MainQuery.ProductLineNo <= 99 AND ( ( CASE WHEN MainQuery.ShipmentDate IS NULL THEN MainQuery.EstimatedDeliveryDate WHEN MainQuery.ShipmentDate <= MainQuery.EstimatedDeliveryDate THEN MainQuery.ShipmentDate ELSE MainQuery.EstimatedDeliveryDate END ) >= @StartDate AND ( CASE WHEN MainQuery.ShipmentDate IS NULL THEN MainQuery.EstimatedDeliveryDate WHEN MainQuery.ShipmentDate <= MainQuery.EstimatedDeliveryDate THEN MainQuery.ShipmentDate ELSE MainQuery.EstimatedDeliveryDate END ) <= @EndDate OR ( MainQuery.EstimatedDeliveryDate >= @StartDate AND MainQuery.EstimatedDeliveryDate <= @EndDate ) ) AND ((MainQuery.OrderStatus = '結案' AND MainQuery.ReceivedDate IS NOT NULL) OR (MainQuery.OrderStatus = '未結')) AND EstimatedDeliveryDate>= @StartDate and EstimatedDeliveryDate<= @EndDate ORDER BY MainQuery.EstimatedDeliveryDate ASC;";
            return await orders.GetBySql(sql, keyValuePairs);

        }

        /// <summary>
        /// 獲取公司數據源。
        /// </summary>
        /// <param name="parameter">訂單查詢參數。</param>
        /// <returns>公司數據源。</returns>
        private async Task<MCompanyDataSource?> GetCompanyDataSource(IUnitOfWork unitOfWork, OrderParameter parameter)
        {
            try
            {
                if (parameter != null)
                {
                    var companyDataRepository = unitOfWork.GetRepository<MCompanyDataSource>();
                    var keyValuePairs = Utility.CreateDynamicParameters(parameter);
                    string sql = GetSqlCommand("CompanyDataSource", "Get");
                    var result = await companyDataRepository.GetBySql(sql, keyValuePairs);
                    return result.SingleOrDefault();
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetCompanyDataSource failed.");
                return new MCompanyDataSource();
            }
        }

        /// <summary>
        /// 獲取逾期訂單列表的實際邏輯。
        /// </summary>
        /// <param name="companyDataSource">公司數據源。</param>
        /// <param name="parameter">訂單查詢參數。</param>
        /// <returns>逾期訂單列表。</returns>
        private async Task<List<Order>?> FetchOrderOverdueListAsync(IUnitOfWork unitOfWork, MCompanyDataSource companyDataSource, OrderParameter? parameter)
        {
            if (parameter == null)
            {
                return null;
            }
            var orders = unitOfWork.GetRepository<Order>();


            var sqlInfo = Utility.FromJson<SqlInfo>(companyDataSource.sqlcmd);
            var keyValuePairs = Utility.CreateDynamicParameters(parameter);
            if (sqlInfo == null) return null;
            return await orders.GetBySql(sqlInfo.OrderOverdueList, keyValuePairs);
        }

        /// <summary>
        /// 更新連接信息。
        /// </summary>
        /// <param name="generic">泛型倉庫實例。</param>
        /// <param name="connectionString">連接字符串。</param>
        private void UpdateConnectionInfo<T>(IRepository<T> generic, string connectionString) where T : class
        {
            var newConnectionInfo = new DbConnectionInfo { ConnectionString = connectionString, DbType = 1 };
            _logger.LogInformation($"Updating connection info: {connectionString}");
            generic.UpdateConnectionInfo(newConnectionInfo);
        }

        /// <summary>
        /// 創建鍵值對集合。
        /// </summary>
        /// <param name="parameter">參數對象。</param>
        /// <returns>鍵值對集合。</returns>
        private ConcurrentDictionary<string, object?>? CreateKeyValuePairs(object parameter)
        {
            try
            {
                var keyValuePairs = new ConcurrentDictionary<string, object?>();
                if (parameter != null)
                {
                    var properties = parameter.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        string key = "@" + property.Name;
                        object? value = property.GetValue(parameter);
                        value ??= DBNull.Value;
                        keyValuePairs.TryAdd(key, value);
                    }
                }
                return keyValuePairs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateKeyValuePairs failed.");
                return null;
            }
        }


        

        /// <summary>
        /// 獲取生產線組列表。
        /// </summary>
        /// <param name="id">生產線組ID。</param>
        /// <returns>生產線組列表。</returns>
        private async Task<List<VProductGp>> GetProductionGroup(IUnitOfWork unitOfWork, int id)
        {

            var vProductGp = unitOfWork.GetRepository<VProductGp>();
            string sql = GetSqlCommand("productionLineGroup", "Get");
            //ConcurrentDictionary<string, object?>? concurrentKeyValuePairs = new ConcurrentDictionary<string, object?>();
            //concurrentKeyValuePairs.TryAdd("@id", id);
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            return await vProductGp.GetBySql(sql, parameters);

            

        }

        /// <summary>
        /// 獲取裝配中心列表。
        /// </summary>
        /// <param name="companyId">公司ID。</param>
        /// <returns>裝配中心列表。</returns>
        private async Task<List<MAssembleCenter>> GetAssembleCenter(IUnitOfWork unitOfWork, int companyId)
        {
            try
            {

                var assemblecenter = unitOfWork.GetRepository<MAssembleCenter>();
                string sql = GetSqlCommand("assemblecenter", "Get");
                //ConcurrentDictionary<string, object?>? concurrentKeyValuePairs = new ConcurrentDictionary<string, object?>();
                //concurrentKeyValuePairs.TryAdd("@company_fk", companyId);
                var parameters = new DynamicParameters();
                parameters.Add("@company_fk", companyId);
                return await assemblecenter.GetBySql(sql, parameters);

               

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAssembleCenter failed.");
                return new List<MAssembleCenter>();
            }
        }

        /// <summary>
        /// 更新訂單列表中的生產線組信息。
        /// </summary>
        /// <param name="orderList">訂單列表。</param>
        /// <param name="assembleCenterList">裝配中心列表。</param>
        private List<Order>? UpdateProductLineGroupWithDictionary(List<Order> orderList, List<VProductGp> assembleCenterList)
        {
            if (orderList != null && assembleCenterList != null)
            {
                var assembleCenterDict = new ConcurrentDictionary<string, string>();
                foreach (var ac in assembleCenterList)
                {
                    assembleCenterDict.TryAdd(ac.plId, ac.sName??"");
                }

                orderList.ForEach(order =>
                {
                    if (assembleCenterDict.TryGetValue(order.ProductLineNo??"", out var sName))
                    {
                        order.ProductLineGroup = sName;
                    }
                });
            }
            return orderList;
        }

        public Task<VOrder?> GetList(IUnitOfWork unitOfWork, OrderParameter? parameter = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 獲取逾期訂單列表。
        /// </summary>
        /// <param name="companyDataSource">公司數據源。</param>
        /// <param name="parameter">訂單查詢參數。</param>
        /// <returns>逾期訂單列表。</returns>
        public async Task<List<Order>?> GetOrderOverdueList(IUnitOfWork unitOfWork, MCompanyDataSource companyDataSource, OrderParameter? parameter = null)
        {
            return await FetchOrderOverdueListAsync(unitOfWork, companyDataSource, parameter);
        }

        /// <summary>
        /// 獲取訂單列表。
        /// </summary>
        /// <param name="companyDataSource">公司數據源。</param>
        /// <param name="parameter">訂單查詢參數。</param>
        /// <returns>訂單列表。</returns>
        public async Task<List<Order>?> GetOrderList(IUnitOfWork unitOfWork, MCompanyDataSource companyDataSource, OrderParameter? parameter = null)
        {
            return await FetchOrderListAsync(unitOfWork, companyDataSource, parameter);
        }
    }
}

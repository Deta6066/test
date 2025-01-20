using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Models;
using VIS_API.Models.CNC;
using VIS_API.Repositories.Interface;
using VIS_API.Service.Interface;
using VIS_API.UnitWork;
using VIS_API.Utilities;
using static VIS_API.Utilities.AllEnum;

namespace VIS_API.Service
{
    /// <summary>
    /// 出貨統計表服務
    /// </summary>
    public class SShipment : ISShipment
    {
        ExceptionHandler _exceptionHandler;
        IServiceProvider _serviceProvider;
        IUnitOfWork _unitOfWork;
        public SShipment(ExceptionHandler exceptionHandler, IServiceProvider serviceProvider, IUnitOfWork unitOfWork)
        {
            _exceptionHandler = exceptionHandler;
            _serviceProvider = serviceProvider;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public async Task<List<MShipmentDetail>> GetShipmentDetailList(MShipmentDetailParamater paramater)
        {
            List<MShipmentDetail> detail_list = new List<MShipmentDetail>();
            List<MShipment> shipment_list = new List<MShipment>();
            //向ERP DB取得出貨統計表
            // string tableName = "machine_info";
            //依據公司ID取得連線字串與sql字串
            string connectionString = GetShipmentConnectString(paramater.CompanyID);
            int dbType = (int)VISDbType.mssql;
            _unitOfWork.UpdateConnectionInfo(connectionString, dbType);
            //  await _unitOfWork.InitializeAsync(); 
            await _unitOfWork.OpenAsyncConnection();
            try
            {
                var repository = _unitOfWork.GetRepository<MShipmentDetail>();

                string startDate = paramater.StartDate.ToString("yyyy-MM-dd");
                string endDate = paramater.EndDate.ToString("yyyy-MM-dd");
                string cols = "*";
                string sql = GetSql();
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@startDate", startDate);
                parameters.Add("@endDate", endDate);
                detail_list = repository.GetBySql(sql, parameters, 0).Result;
                List<string> customerList = detail_list.Select(x => x.CustomerName).Distinct().ToList();
            }
            catch (Exception ex)
            {
               await _unitOfWork.RollbackAsync();
                throw ex;
            }
            finally
            {
               await _unitOfWork.DisposeAsync();
            }
            // });
            //向VIS DB取得產線名稱

            return detail_list;
        }

        private static string GetShipmentConnectString(int CompanyID)
        {
            return "Data Source=192.168.1.210;Database=FJWSQL;User Id=DEK;Password=asus54886961;TrustServerCertificate=true;";
        }

        private static string GetSql()
        {
            string productionLineNumberAlias = "ProductionLineNumber";
            string customerNameAlias = "CustomerName";
            string itemNoAlias = "ItemNo";
            string itemNameAlias = "ItemName";
            string totalCountAlias = "TotalCount";
            string sql = $"SELECT top (1000) item_22.PLINE_NO AS {productionLineNumberAlias}, cust.custnm2 AS {customerNameAlias},INVOSUB.ITEM_NO AS {itemNoAlias},INVOSUB.itemnm AS {itemNameAlias},INVOSUB.QUANTITY AS {totalCountAlias} FROM INVOSUB LEFT JOIN CUST AS cust ON cust.CUST_NO=INVOSUB.CUST_NO LEFT JOIN CORDSUB AS cordsub ON cordsub.TRN_NO=INVOSUB.CORD_NO AND cordsub.SN=INVOSUB.CORD_SN LEFT JOIN item_22 AS item_22 ON INVOSUB.ITEM_NO=item_22.ITEM_NO LEFT JOIN item AS item ON INVOSUB.ITEM_NO=item.ITEM_NO WHERE invosub.TRN_DATE>=@startDate AND invosub.TRN_DATE<=@endDate AND item.class>='Z' AND item.class<= 'ZR' AND 1<= item_22.pline_no and item_22.pline_no <=99 AND invosub.TYPE='1'";
            return sql;
        }

        private void UpdateConnectionInfo<T>(IRepository<T> generic, string connectionString, int dbType = 0) where T : class
        {
            var newConnectionInfo = new DbConnectionInfo { ConnectionString = connectionString, DbType = dbType };
            //_logger.LogInformation($"Updating connection info: {connectionString}");
            generic.UpdateConnectionInfo(newConnectionInfo);
        }
    }
}

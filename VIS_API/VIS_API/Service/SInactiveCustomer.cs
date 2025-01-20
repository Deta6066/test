using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Models;
using VIS_API.Models.SLS;
using VIS_API.Repositories.Interface;
using VIS_API.Service.Interface;
using VIS_API.UnitWork;
using VIS_API.Utilities;
using static VIS_API.Utilities.AllEnum;

namespace VIS_API.Service
{

    public class SInactiveCustomer : ISInactiveCustomer
    {
        ExceptionHandler _exceptionHandler;
        IServiceProvider _serviceProvider;
        IUnitOfWork _unitOfWork;
        public SInactiveCustomer(ExceptionHandler exceptionHandler, IServiceProvider serviceProvider, IUnitOfWork unitOfWork)
        {
            _exceptionHandler = exceptionHandler;
            _serviceProvider = serviceProvider;
            _unitOfWork = unitOfWork;

        }

        public async Task<List<MInactiveCustomer>> GetMInactiveCustomers(MInactiveCustomerParamater paramater)
        {
       
            List<MInactiveCustomer> list = new List<MInactiveCustomer>();
            string CustomerNameAlias = "CustomerName";
            string LastTransactionDate = "LastTradeDate";
            string InactiveDays = "InactiveDays";
            string InactiveDaysOption = paramater.InactiveDaysOption == 0 ? "<" : ">";

            string sql = $"SELECT * FROM (SELECT cust.CUSTNM2 AS {CustomerNameAlias},max(TRN_DATE) AS {LastTransactionDate},DATEDIFF(DAY, max(TRN_DATE), convert(varchar(10), getdate(), 112)) AS {InactiveDays} FROM cord LEFT JOIN CUST AS cust ON cust.CUST_NO=cord.CUST_NO WHERE cust.CUSTNM2 IS NOT NULL GROUP BY cust.CUSTNM2) AS A WHERE {InactiveDays} {InactiveDaysOption}  @InactiveDays and  {LastTransactionDate}>@startDate And {LastTransactionDate} < @endDate  ORDER BY {LastTransactionDate} DESC";
           try
            {
                string connectionString = GetShipmentConnectString(1);
                int dbType = (int)VISDbType.mssql;
                _unitOfWork.UpdateConnectionInfo(connectionString, dbType);
                await _unitOfWork.InitializeAsync();
                //IRepository<MInactiveCustomer> repository = _serviceProvider.GetRequiredService<IRepository<MInactiveCustomer>>();
                IGenericRepositoryBase<MInactiveCustomer> repository = _unitOfWork.GetRepository<MInactiveCustomer>();
              
                int _inactiveDays = paramater.InactiveDays;
                
                var parameters = new DynamicParameters();
                parameters.Add("@InactiveDays", _inactiveDays);
                parameters.Add("@startDate", paramater.StartDate.ToString("yyyy-MM-dd"));
                parameters.Add("@endDate", paramater.EndDate.ToString("yyyy-MM-dd"));

                repository.GetBySql(sql, parameters).Result.ForEach(x => list.Add(x));
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
            return list;
        }
        private void UpdateConnectionInfo<T>(IRepository<T> generic, string connectionString, int dbType = 0) where T : class
        {
            var newConnectionInfo = new DbConnectionInfo { ConnectionString = connectionString, DbType = dbType };
            //_logger.LogInformation($"Updating connection info: {connectionString}");
            generic.UpdateConnectionInfo(newConnectionInfo);
        }
        private static string GetShipmentConnectString(int CompanyID)
        {
            return "Data Source=192.168.1.210;Database=FJWSQL;User Id=DEK;Password=asus54886961;TrustServerCertificate=true;";
        }
    }
}

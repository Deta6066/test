using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
    /// 機台資訊Service
    /// </summary>
    public class SMachineInfo : ISMachineInfo
    {
        IServiceProvider _serviceProvider;
        private readonly ExceptionHandler _exceptionHandler;
        // private readonly ILogger<SMachineInfo> _logger;
        private IRMachineInfo _rMachineInfo;
        IUnitOfWork _unitOfWork;
        public SMachineInfo(IRMachineInfo rMachineInfo, ExceptionHandler exceptionHandler, ILogger<SMachineInfo> logger,IServiceProvider serviceProvider, IUnitOfWork unitOfWork)
        {
            _rMachineInfo = rMachineInfo;
            _exceptionHandler = exceptionHandler;
           // _logger = logger;
           _serviceProvider = serviceProvider;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 取得設備資訊
        /// </summary>
        /// <param name="companyDataSource"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<List<MMachineInfo>> GetMachineInfo(MCompanyDataSource companyDataSource, MMachineInfoParameter parameter)
        {
            string tableName = "machine_info";
            string connectionString = "Server=192.168.101.171;port=3306;Database=cnc_db;User Id=root;Password=dek54886961;"; //未使用到 先註解by秋
            List<MMachineInfo> machineInfo = new List<MMachineInfo>();
            try
            {
                _unitOfWork.UpdateConnectionInfo(connectionString, (int)VISDbType.mysql);
               await _unitOfWork.OpenAsyncConnection();
                IGenericRepositoryBase<MMachineInfo> repository = _rMachineInfo;
                //UpdateConnectionInfo(repository, connectionString);
                //var sqlInfo = Utility.FromJson<SqlInfo>(companyDataSource.sqlcmd);
                //  var cols = Utilities.Utility.CmdColStr(new MMachineInfo() { ID=0, acts="string"});
                string cols = "*";
                string sql = $"SELECT {cols} FROM {tableName}";
                
                var parameters = new DynamicParameters();
                parameters.Add("area_name", parameter.AreaName);
                machineInfo = await repository.GetBySql(sql, parameters);
            }
            catch (Exception ex)
            {
            }
            return machineInfo;

        }
        /// <summary>
        /// 更新連接信息。
        /// </summary>
        /// <param name="generic">泛型倉庫實例。</param>
        /// <param name="connectionString">連接字符串。</param>
        private void UpdateConnectionInfo<T>(IRepository<T> generic, string connectionString) where T : class
        {
            var newConnectionInfo = new DbConnectionInfo { ConnectionString = connectionString, DbType = 0 };
            //_logger.LogInformation($"Updating connection info: {connectionString}");
            generic.UpdateConnectionInfo(newConnectionInfo);
        }
        /// <summary>
        /// 創建鍵值對集合。
        /// </summary>
        /// <param name="parameter">參數對象。</param>
        /// <returns>鍵值對集合。</returns>
        private ConcurrentDictionary<string, object>? CreateKeyValuePairs(object parameter)
        {
            var keyValuePairs = new ConcurrentDictionary<string, object>();
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
    }
}

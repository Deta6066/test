using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models;
using VisLibrary.Models.CNC;
using VisLibrary.Repositories.Interface;
using VisLibrary.Service.Interface;
using VisLibrary.UnitWork;
using VisLibrary.Utilities;
using static VisLibrary.Utilities.AllEnum;

namespace VisLibrary.Service
{
    /// <summary>
    /// 可視化資訊Service
    /// </summary>
    public class SAps_info : ISAps_info
    {
        IServiceProvider _serviceProvider;
        IUnitOfWork _unitOfWork;
        private readonly ExceptionHandler _exceptionHandler;
        public SAps_info(IServiceProvider serviceProvider, ExceptionHandler exceptionHandler, IUnitOfWork unitOfWork)
        {
            _serviceProvider = serviceProvider;
            _exceptionHandler = exceptionHandler;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="companyID">
        /// <inheritdoc/>
        /// </param>
        /// <returns></returns>
        public List<MAps_info> GetAps_infoList(int companyID = 0)
        {
            List<MAps_info> aps_infoList = new List<MAps_info>();
            string connectionString = "Server=192.168.101.171;port=3306;Database=cnc_db;User Id=root;Password=dek54886961;";
            string sql= "SELECT * FROM aps_info";
            _unitOfWork.UpdateConnectionInfo(connectionString, (int)VISDbType.mysql);
            var repository = _unitOfWork.GetRepository<MAps_info>();
            _exceptionHandler.ExecuteWithExceptionHandling(() =>
            {
                aps_infoList = repository.GetBySql(sql, new()).Result;
            });
            return aps_infoList;
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

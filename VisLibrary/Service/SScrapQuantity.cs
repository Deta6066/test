using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Business.Interface;
using VisLibrary.Models;
using VisLibrary.Models.WHE;
using VisLibrary.Repositories.Interface;
using VisLibrary.Service.Interface;
using VisLibrary.UnitWork;
using VisLibrary.Utilities;
using static VisLibrary.Utilities.AllEnum;
using static VisLibrary.Utilities.ApiUtility;

namespace VisLibrary.Service
{
    public class SScrapQuantity : IScrapQuantity
    {
        IConfiguration _configuration;
      //  private readonly IServiceProvider _serviceProvider;
        ExceptionHandler _exceptionHandler;
        IUnitOfWork _unitOfWork;
        /// <summary>
        /// 是否為localDb測試
        /// </summary>
        bool IsTest = true;
        const string TableName = "scrap_quantity";
        public SScrapQuantity(ExceptionHandler exceptionHandler,  IConfiguration configuration, IUnitOfWork unitOfWork)
        {
           // _rScrapQuantity = rScrapQuantity;
            _configuration = configuration;
            //_repository = repository;
            _exceptionHandler = exceptionHandler;
            //_serviceProvider = serviceProvider;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<MScrapQuantity>> GetScrapQuantityList(int companyID, MScrapQuantityParameter? filter)
        {
            //更新連線資訊
            var _dataSource = UpdateDbInfo(companyID, IsTest).Result;
            List<MScrapQuantity> result = new List<MScrapQuantity>();
            if (_dataSource == null)
            {
                return result;
            }
            else
            {
                if(_dataSource.sourceType == (int)VISDbType.mssql)
                {
                    return await GetScrapQuantityListBySqlCmd(_dataSource,companyID, filter);
                }
                if (_dataSource.sourceType == (int)VISDbType.other)
                {
                    return _exceptionHandler.ExecuteWithExceptionHandling<List<MScrapQuantity>>(() => _unitOfWork.RScrapQuantity.GetScrapQuantityList(companyID, filter)) ?? new List<MScrapQuantity>();
                }
                if (_dataSource.sourceType == (int)VISDbType.api)
                {
                    MApiSource? _apiSource = Utility.FromJson<MApiSource>(_dataSource.apiSource);
                    MApiInfo mApiInfo = _apiSource.GetScrapQuantityList;
                    result = GetDataByAPIAsync<List<MScrapQuantity>>(mApiInfo, companyID, filter).Result;
                    return result;
                }
            }
            return result;
        }
        /// <summary>
        /// 取得報廢資料明細表BySqlCmd
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="companyID"></param>
        /// <param name="filter"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async Task<List<MScrapQuantity>> GetScrapQuantityListBySqlCmd(MCompanyDataSource dataSource, int companyID, MScrapQuantityParameter? filter)
        {
            List<MScrapQuantity> result = new List<MScrapQuantity>();
            //IRepository<MScrapQuantity> repository = _serviceProvider.GetRequiredService<IRepository<MScrapQuantity>>();
            DbConnectionInfo dbConnectionInfo = Utility.GetDbInfoByDataSource(dataSource);
            _unitOfWork.UpdateConnectionInfo(dbConnectionInfo.ConnectionString, dbConnectionInfo.DbType);
            // await _unitOfWork.InitializeAsync();
            await _unitOfWork.OpenAsyncConnection();
            var repository = _unitOfWork.GetRepository<MScrapQuantity>();
            SqlInfo sqlInfo = Utility.FromJson<SqlInfo>(dataSource.sqlcmd);
            string sql=sqlInfo.GetScrapQuantityList;
            //string temp = $"SELECT ITEMIO.STOCKNAME AS 報廢者,ITEMIOS.TRN_NO AS 單據號碼, ITEMIOS.TRN_DATE AS 單據日期, ITEM.ITEM_NO AS 品號,ITEM.ITEMNM AS 品名規格, ITEM.UNIT AS 單位, ITEM.SCOST AS 標準成本 , ITEMIOS.IRQTY AS 報廢數量, (ITEM.SCOST*ITEMIOS.IRQTY) AS 金額小計, ITEMIO.REMARK AS 備註 FROM ITEM AS ITEM LEFT JOIN ITEMIOS AS ITEMIOS ON ITEM.ITEM_NO = ITEMIOS.ITEM_NO AND ITEMIOS.IO='3' LEFT JOIN ITEMIO AS ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO LEFT JOIN DEP AS DEP ON ITEMIO.D_NO=DEP.S_NO WHERE ITEMIOS.IRQTY <> 0 AND ITEMIOS.WASTES_SW2='Y' AND ITEMIOS.TRN_DATE >= {0} AND ITEMIOS.TRN_DATE <= {1}  {2}   order by 金額小計 desc";
            if (sqlInfo == null)
            {
                return result;
            }
            else
            {
                //ConcurrentDictionary<string, object?> paramaterList= new ConcurrentDictionary<string, object?>();
                var parameters = new DynamicParameters();
                if (filter != null)
                {
                    if (filter.StartDate != null)
                    {
                        //paramaterList.TryAdd("@startDate", filter.StartDate.ToShortDateString());
                        //filter.StartDate轉成"yyyyMMdd"
                        //paramaterList.TryAdd("@startDate", filter.StartDate.ToString("yyyyMMdd"));
                        parameters.Add("@startDate", filter.StartDate.ToString("yyyyMMdd"));

                    }
                    if (filter.EndDate != null)
                    {
                        //paramaterList.TryAdd("@endDate", filter.EndDate);
                        //filter.EndDate轉成"yyyyMMdd"
                        //paramaterList.TryAdd("@endDate", filter.EndDate.ToString("yyyyMMdd"));
                        parameters.Add("@endDate", filter.EndDate.ToString("yyyyMMdd"));

                    }

                }
                result = await repository.GetBySql(sql, parameters, 0);
            }
            return result;

        }

        /// <summary>
        /// 新增報廢數量
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="scrapQuantity"></param>
        /// <returns>
        /// 回傳新增的筆數
        /// </returns>
        public async Task<int> AddScrapQuantity(int companyID, MScrapQuantity scrapQuantity)
        {
            //更新連線資訊
           await UpdateDbInfo(companyID, IsTest);
            //int result = await  _rScrapQuantity.Insert(scrapQuantity);
            int result= await _unitOfWork.RScrapQuantity.Insert(scrapQuantity,TableName);
            return result;
        }
        /// <summary>
        /// 新增報廢數量(批量)
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="scrapQuantityList"></param>
        /// <returns>
        /// 回傳新增的筆數
        /// </returns>
        public async Task<int> AddScrapQuantity(int companyID, List<MScrapQuantity> scrapQuantityList)
        {
            //更新連線資訊
            int result = 0;
            UpdateDbInfo(companyID, IsTest);
            foreach (var scrapQuantity in scrapQuantityList)
            {
                scrapQuantity.company_fk = companyID;
                result += await _unitOfWork.RScrapQuantity.Insert(scrapQuantity, TableName);
            }
            await _exceptionHandler.ExecuteWithExceptionHandlingAsync(async () =>
            {
                result = await AddScrapQuantityListAsync(companyID, scrapQuantityList, result);
            }, ex => Console.WriteLine($"Error in AddScrapQuantity: {ex.Message}"));
            return result;
        }
        /// <summary>
        /// 新增報廢數量(批量)
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="scrapQuantityList"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<int> AddScrapQuantityListAsync(int companyID, List<MScrapQuantity> scrapQuantityList, int result)
        {
            foreach (var scrapQuantity in scrapQuantityList)
            {
                scrapQuantity.company_fk = companyID;
                result += await _unitOfWork.RScrapQuantity.Insert(scrapQuantity, TableName);
            }

            return result;
        }

        /// <summary>
        /// 刪除報廢數量
        /// </summary>
        /// <param name="scrapQuantityID"></param>
        /// <returns></returns>
        public async Task<int> DeleteScrapQuantity(int companyID, int scrapQuantityID)
        {
            //更新連線資訊
            UpdateDbInfo(companyID, IsTest);
            int result = await _unitOfWork.RScrapQuantity.Delete(scrapQuantityID, TableName);
            return result;
        }
        /// <summary>
        /// 更新報廢數量
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="scrapQuantity"></param>
        /// <returns></returns>
        public async Task<int> UpdateScrapQuantity(int companyID, MScrapQuantity scrapQuantity)
        {
            //更新連線資訊
            int result = 0;
            UpdateDbInfo(companyID, IsTest);
            //使用ExecuteWithExceptionHandlingAsync處理異常
            result = await _exceptionHandler.ExecuteWithExceptionHandlingAsync<int>(async () =>
            {
                return await _unitOfWork.RScrapQuantity.Update(scrapQuantity, TableName);
            }, ex => Console.WriteLine($"Error in UpdateScrapQuantity: {ex.Message}"));
            return result;
        }
        private async Task<MCompanyDataSource> UpdateDbInfo(int companyID, bool isTest = false)
        {
            //取得公司資料來源
            MCompanyDataSource data_source = await _unitOfWork.RCompanyDataSource.GetByPk(companyID);

            //更新資料來源
            DbConnectionInfo dbConnectionInfo = new DbConnectionInfo() { DbType = (int)VISDbType.mysql };
            if (isTest)
            {
                #region 從configuration取得資料庫連線資訊
                dbConnectionInfo = new DbConnectionInfo()
                {
                    DbType = (int)VISDbType.mysql,
                    ConnectionString = _configuration.GetConnectionString("MySqlConnectionString")
                };
                #endregion
            }
            else
            {
                #region 依據資料來源取得資料庫連線資訊
                dbConnectionInfo = Utility.GetDbInfoByDataSource(data_source);
                #endregion
            }
           // _unitOfWork.RScrapQuantity.UpdateConnectionInfo(dbConnectionInfo);
            return data_source ?? new MCompanyDataSource();
        }
    }
}

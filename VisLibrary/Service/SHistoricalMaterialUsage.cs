using VisLibrary.Models;
using VisLibrary.Models.PCD;
using VisLibrary.Service;

using VisLibrary.Repositories.Interface;
using VisLibrary.Service.Interface;
using VisLibrary.Business.Interface;
using VisLibrary.Repositories;
using VisLibrary.Utilities;
using Microsoft.Extensions.Configuration;
using static VisLibrary.Utilities.AllEnum;
using static VisLibrary.Utilities.ApiUtility;
using NLog.Filters;
using VisLibrary.UnitWork;

namespace VisLibrary.Service
{
    /// <inheritdoc/>
    public class SHistoricalMaterialUsage : ISHistoricalMaterialUsage
    {
        ISCompany _sCompany;
        IConfiguration _configuration;
        ExceptionHandler _exceptionHandler;
        IUnitOfWork _unitOfWork;
        public SHistoricalMaterialUsage(ExceptionHandler exceptionHandler,  ISCompany sCompany, IConfiguration configuration, IRCompanyDataSource companyDataSource, IUnitOfWork unitOfWork)
        {
            _sCompany = sCompany;
            _configuration = configuration;
            _exceptionHandler = exceptionHandler;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<HistoricalMaterialUsageModel>> GetHistoricalMaterials(MHistoricalMaterialUsageParameter filter, int CompanyID)
        {
            //取得公司所有的領料記錄，並依照篩選器模型進行篩選，統計為歷史用料統計表
            List<HistoricalMaterialUsageModel_Detail> detail_list = await GetHistoricalMaterialUsageModel_Detail(filter);
            List<HistoricalMaterialUsageModel> list = new List<HistoricalMaterialUsageModel>();
            //透過品號分組，取得品號的歷史用量
            //Key:品號 Value:歷史用量
            //去除detail_list的ProductionOrderDetailID為空的資料
            detail_list = detail_list.FindAll(x => !string.IsNullOrEmpty(x.ProductionOrderDetailID));
            //透過品號分組
            Dictionary<string, List<HistoricalMaterialUsageModel_Detail>> DetailDic = detail_list.GroupBy(x => x.ProductionOrderDetailID).ToDictionary(x => x.Key, x => x.ToList());
            //透過品號取得歷史用量
            foreach (var group in DetailDic)
            {
                list.Add(new HistoricalMaterialUsageModel()
                {
                    //品號
                    MaterialNo = group.Key,
                    //品名規格
                    MaterialDescription = group.Value[0].MaterialDescription,
                    //用途數量統計
                    UsageCategory = new Dictionary<string, int>(),
                });
                foreach (var detail in group.Value)
                {
                    //計算用途名稱的數量
                    if (list[list.Count - 1].UsageCategory.ContainsKey(detail.UsageDescription))
                    {
                        //如果用途名稱已存在，則數量增加
                        list[list.Count - 1].UsageCategory[detail.UsageDescription] += detail.MaterialQuantity;
                    }
                    else
                    {
                        //如果用途名稱不存在，則新增用途名稱
                        list[list.Count - 1].UsageCategory.Add(detail.UsageDescription, detail.MaterialQuantity);
                    }

                }


            }
            return list;
        }
        /// <summary>
        /// 取得領料紀錄
        /// </summary>
        /// <param name="CompanyID">公司ID</param>
        /// <param name="MaterialNo">品號</param>
        /// <returns></returns>
        public async Task<List<HistoricalMaterialUsageModel_Detail>> GetHistoricalMaterialUsageModel_Detail(MHistoricalMaterialUsageParameter filter)
        {
            List<HistoricalMaterialUsageModel_Detail> list = new List<HistoricalMaterialUsageModel_Detail>();
            #region 正式環境
            //透過公司ID取得公司來源
          // await _unitOfWork.InitializeAsync();
          await _unitOfWork.OpenAsyncConnection();
            MCompanyDataSource _dataSource = await _unitOfWork.RCompanyDataSource.GetByPk(filter.CompanyID) ?? new MCompanyDataSource();
            await _unitOfWork.CommitAsync();
            //從公司來源取得SourceType
            int _sourceType = _dataSource.sourceType;
            #endregion
            #region local 測試
            //MCompanyDataSource _dataSource = new MCompanyDataSource();
            //_dataSource.dbParamater = _configuration.GetConnectionString("MySqlConnectionString");
            //_dataSource.sourceType = (int)VISDbType.mysql;
            #endregion
            //判斷資料來源是否為API
            if (_dataSource.sourceType == (int)VISDbType.api)
            {
                //透過API取得資料
                MApiSource? _apiSource = Utility.FromJson<MApiSource>(_dataSource.apiSource);
                MApiInfo mApiInfo = _apiSource.GetHistoricalMaterialUsageModel_Detail;
                Dictionary<string, object> headers = new Dictionary<string, object>();
                headers.Add("CompanyID", filter.CompanyID);
                headers.Add("MaterialNo", filter.MaterialNo);
                list = GetDataByAPIAsync<List<HistoricalMaterialUsageModel_Detail>>(mApiInfo, headers: headers, null).Result;
            }
            else
            {
                //DbConnectionInfo _dbInfo = Utility.GetDbInfoByDataSource(_dataSource);
                DbConnectionInfo _dbInfo = new DbConnectionInfo()
                {
                    DbType = (int)VISDbType.mssql,
                    //ConnectionString = _configuration.GetConnectionString("MssqlConnectionString")
                    ConnectionString = _dataSource.dbParamater
                };
                //從HistoricalMaterialUsageRepositories取得Repository
                _unitOfWork.UpdateConnectionInfo(_dbInfo.ConnectionString,_dbInfo.DbType);
                try
                {
                    //取得領料紀錄
                 //  await _unitOfWork.InitializeAsync();
                    list = _unitOfWork.RHistoricalMaterialUsage.GetHistoricalMaterialUsageModel_DetailAsync(filter).Result;
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
            }

            return list;
        }
    }
}

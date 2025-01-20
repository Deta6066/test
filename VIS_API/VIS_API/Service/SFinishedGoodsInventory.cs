using VIS_API.Models;

using VIS_API.Repositories.Interface;
using VIS_API.Models.WHE;
using VIS_API.Service.Interface;
using VIS_API.Utilities;
using static VIS_API.Utilities.AllEnum;
using static VIS_API.Utilities.ApiUtility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VIS_API.Models.View;
using System.Collections.Generic;
using VIS_API.UnitWork;
using System.Collections.Concurrent;
using Dapper;

namespace VIS_API.Service
{
    /// <summary>
    /// 成品庫存Service
    /// </summary>
    public class SFinishedGoodsInventory : ISFinishedGoodsInventory
    {
        IConfiguration _configuration;
        IUnitOfWork _unitOfWork;
        ExceptionHandler _exceptionHandler;
        Dictionary<int, IRFinishedGoodsInventory> _inventoryRepositories = new Dictionary<int, IRFinishedGoodsInventory>();
        public SFinishedGoodsInventory(ExceptionHandler exceptionHandler, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            //_companyDataSource = rCompanyDataSource;
            _configuration = configuration;
            _exceptionHandler = exceptionHandler;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 取得成品庫存明細
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<VMFinishGoodDetail>> GetFinishedGoodsInventoryDetail(int companyID, MInventoryParameter filter)
        {
            List<MFinishGoodDetail>? result = new List<MFinishGoodDetail>();
            //依照公司ID取得更新資料庫連線資訊
            var _dataSource = await UpdateDbInfo(companyID, false);
          // await _unitOfWork.InitializeAsync();
          await _unitOfWork.OpenAsyncConnection();
            //判斷SourceType是否為API
            if (_dataSource.sourceType == (int)VISDbType.api)
            {
                MApiSource? _apiSource = Utility.FromJson<MApiSource>(_dataSource.apiSource);
                if (_apiSource != null)
                {
                    MApiInfo mApiInfo = _apiSource.GetFinishedGoodsInventoryDetails;
                    result = GetDataByAPIAsync<List<MFinishGoodDetail>>(mApiInfo, companyID, filter).Result;
                }
            }
            else
            {
               try
                {
                    //透過Repository取得庫存明細
                 string _sql= await _unitOfWork.RFinishedGoodsInventory.GetFinishedGoodsInventoryDetailSql(filter, companyID, _dataSource);
                   
                    var parameters = new DynamicParameters();
                    parameters.Add("@company_fk", companyID);
                    if (filter != null)
                    {
                        if (!string.IsNullOrEmpty(filter.Factory_fk))
                        {
                            parameters.Add("@factory_fk", filter.Factory_fk);
                        }
                    }
                    result = await _unitOfWork.RFinishedGoodsInventory.GetBySql(_sql, parameters, 0);
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
            List<VMFinishGoodDetail> vmResult = new List<VMFinishGoodDetail>();
            vmResult = modelToViewModel(result);
            return vmResult;
        }

        private List<VMFinishGoodDetail> modelToViewModel(List<MFinishGoodDetail>?  list)
        {
            if (list == null)
            {
                return new List<VMFinishGoodDetail>();
            }
            //把Model轉成ViewModel
            List<VMFinishGoodDetail> vmList = new List<VMFinishGoodDetail>();
            foreach (MFinishGoodDetail detail in list)
            {
                VMFinishGoodDetail vmDetail = new VMFinishGoodDetail();
                vmDetail.CustomerName = detail.CustomerName;
                vmDetail.Factory_fk = detail.Factory_fk;
                vmDetail.ManufacturingBatchNumber = detail.ManufacturingBatchNumber;
                vmDetail.ProductionLineNumber = detail.ProductionLineNumber;
                vmDetail.ReceiptDate = detail.ReceiptDate;
                vmDetail.StockDays = detail.StockDays;
                vmDetail.StockReason = detail.StockReason;
                vmDetail.StockCost = detail.StockCost;
                vmList.Add(vmDetail);
            }
            return vmList;
        }

        /// <summary>
        /// 更新資料庫連線資訊
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="isTest">
        /// 是否使用測試資料庫
        /// </param>
        private async Task<MCompanyDataSource> UpdateDbInfo(int companyID, bool isTest = false)
        {
            _unitOfWork.UpdateConnectionInfo(_configuration.GetConnectionString("MySqlConnectionString"), (int)VISDbType.mysql);
            //透過公司ID取得公司資料來源
            MCompanyDataSource? companyDataSource = await _unitOfWork.RCompanyDataSource.GetByPk(companyID);
            //await _unitOfWork.CommitAsync(); 
            if (companyDataSource == null)
            {
                return new MCompanyDataSource();
                throw new Exception("CompanyDataSource is null");
            }
            DbConnectionInfo dbInfo = new DbConnectionInfo() { DbType = companyDataSource.dbType, ConnectionString = companyDataSource.dbParamater };
            if (isTest)
            {
                #region local測試用
                dbInfo = new DbConnectionInfo() { DbType = (int)VISDbType.mysql, ConnectionString = _configuration.GetConnectionString("MySqlConnectionString") };
                #endregion
            }
            else
            {
                dbInfo = Utility.GetDbInfoByDataSource(companyDataSource);
                _unitOfWork.UpdateConnectionInfo(dbInfo.ConnectionString, dbInfo.DbType);
            }

            //更新連線資訊
            #region 透過公司ID取得資料來源
            //_inventoryRepository.UpdateConnectionInfo(dbInfo.ConnectionString, dbInfo.DbType);
            #endregion
            return companyDataSource;
        }
    }
}

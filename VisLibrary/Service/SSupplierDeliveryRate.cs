using VisLibrary.Models;
using Repositories;
using VisLibrary.Service;
using System.ComponentModel.Design;

using VisLibrary.Repositories.Interface;
using Service.Interface;
using VisLibrary.Business.Interface;
using VisLibrary.Utilities;
using Microsoft.Extensions.Configuration;
using static VisLibrary.Utilities.AllEnum;
using static VisLibrary.Utilities.ApiUtility;
using VisLibrary.Service.Interface;
using VisLibrary.Repositories;
using Newtonsoft.Json;
using System.Text;
using VisLibrary.Models.WHE;
using System.Collections.Concurrent;
using VisLibrary.UnitWork;

namespace VisLibrary.Service
{
    public class SSupplierDeliveryRate : ISSupplierDeliveryRate
    {
        ISCompany _companyService;
        IRSupplierScore _supplierScoreRepository;
        IConfiguration _configuration;
        ISCompanyDataSource _companyDataSourceService;
        ExceptionHandler _exceptionHandler;
        IUnitOfWork _unitOfWork;
        public SSupplierDeliveryRate(ExceptionHandler exceptionHandler, IRSupplierScore rSupplierScore, ISCompany sCompany, IConfiguration configuration, ISCompanyDataSource companyDataSourceService, IUnitOfWork unitOfWork)
        {
            //_db = db;
            //_companyService = new SCompany(_db);
            //_companyDataSourceService = new SCompanyDataSource(_db);
            //_SupplierScoreRepositories = InitRepository();
            _companyDataSourceService = companyDataSourceService;
            _supplierScoreRepository = rSupplierScore;
            _companyService = sCompany;
            _configuration = configuration;
            _companyDataSourceService = companyDataSourceService;
            _exceptionHandler = exceptionHandler;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 取得供應商達交率
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<MSupplierScore>> GetSupplierDeliveryRate(int companyID, MSupplierScoreParameter filter)
        {
            List<MSupplierScore> result = new List<MSupplierScore>();
            //透過公司ID取得供應商達交率
            //取得達交率明細列表，依供應商名稱統計達交率模型
            List<MSupplierScoreDetail> deliveryRateDetail = await GetSupplierDeliveryRateDetail(companyID, filter);
            //依供應商名稱分組，統計採購次數，期限內交貨次數，採購總數量，達交率
            //遍歷供應商名稱
            //取得供應商名稱清單
            var supplierNames = deliveryRateDetail.Select(x => x.FactoryName).Distinct();
            //將供應商名稱加入result
            result = supplierNames.Select(x => new MSupplierScore() { SupplierName = x }).ToList();
            //將符合供應商名稱的達交率明細加入result
            foreach (var item in deliveryRateDetail)
            {
                if (result.Exists(x => x.SupplierName == item.FactoryName))
                {
                    MSupplierScore supplierScoreModel = result.Find(x => x.SupplierName == item.FactoryName);
                    //期限內交貨次數
                    supplierScoreModel.OnTimeDelivery += item.DeadlineDeliveryQuantity;
                    //採購次數
                    //scoreModel.PurchaseTimes = deliveryRateDetail.FindAll(x => x.SupplierName == item.SupplierName).Count();
                    supplierScoreModel.PurchaseTimes += 1;
                    //採購總數量
                    supplierScoreModel.TotalPurchase += item.PurchasingQuantity;
                    //達交率 = 期限內交貨次數 / 採購次數
                    supplierScoreModel.DeliveryRate = (double)supplierScoreModel.OnTimeDelivery / supplierScoreModel.TotalPurchase;
                }
            }
            return result;
        }

        /// <summary>
        /// 取得供應商達交率明細
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<MSupplierScoreDetail>> GetSupplierDeliveryRateDetail(int companyID, MSupplierScoreParameter filter)
        {
            MCompanyDataSource dataSource = await UpdateDbInfoAsync(companyID, false);
            List<MSupplierScoreDetail> result = new List<MSupplierScoreDetail>();
            #region 欄位別名
            //供應商名稱別名
            string FactoryNameAlias = "FactoryName";
            //採購單號別名
            string TransactionNumberAlias = "TransactionNumber";
            //採購明細序別名
            string SerialNumberAlias = "SerialNumber";
            //品號別名
            string ItemNumberAlias = "ItemNumber";
            //品名規格別名
            string ItemNameSpecificationAlias = "ItemName";
            //採購數量別名
            string PurchasingQuantityAlias = "PurchasingQuantity";
            //期限內已交數量別名
            string DeadlineDeliveryQuantityAlias = "DeadlineDeliveryQuantity";
            //採購單日期別名
            string TransactionDateAlias = "TransactionDate";
            //預交日期別名
            string AdvanceDeliveryDateAlias = "EstimatedDeliveryDate";
            //期限內最後進貨日期別名
            string DeadlineLastPurchaseDateAlias = "QuantityDelivered_Deadline";
            //達交率別名
            string DeliveryRateAlias = "DeliveryRate";
            #endregion
            //判斷資料來源是否為api
            if (dataSource.sourceType == (int)VISDbType.api)
            {
                //if 資料來源為api
                try
                {
                    //取得api相關資料
                    result = GetSupplierDeliveryRateDetailByApi(companyID, filter, dataSource);
                }
                catch (System.Exception ex)
                {
                   // await _unitOfWork.RollbackAsync();
                    throw ex;
                }
                //}, (ex) =>
                //{
                //    Console.WriteLine($"取得供應商達交率明細api異常:{ex.Message}");
                //});
            }
            else
            {
                try
                {
                    //透過Repository取得資料
                    //result = _supplierScoreRepository.GetSupplierDeliveryRateDetail(companyID, filter.SupplierName, filter);
                    string sql= $@"select a.* ,CONVERT(NUMERIC(17, 2), CONVERT(FLOAT, Round(((CONVERT(NUMERIC(17, 2), a.{DeadlineDeliveryQuantityAlias})) / CONVERT(NUMERIC(17, 2), a.{PurchasingQuantityAlias})) * 100, 2))) AS {DeliveryRateAlias} from ( SELECT  fact.factnm2 AS {FactoryNameAlias},sord.trn_no AS {TransactionNumberAlias},  sordsub.sn AS {SerialNumberAlias},    sordsub.item_no AS {ItemNumberAlias},  sordsub.NAME AS {ItemNameSpecificationAlias},  sordsub.quantity AS {PurchasingQuantityAlias},  sum(purcsub.q_delied) AS {DeadlineDeliveryQuantityAlias},CONVERT(datetime, sord.trn_date, 112)   AS {TransactionDateAlias},CONVERT(datetime, sordsub.d1, 112)   AS {AdvanceDeliveryDateAlias},CONVERT(datetime, max(purcsub.trn_date), 112)   AS {DeadlineLastPurchaseDateAlias} FROM sord LEFT JOIN sordsub ON sord.trn_no = sordsub.trn_no LEFT JOIN purcsub ON purcsub.sord_no = sord.trn_no LEFT JOIN fact ON fact.fact_no = sord.fact_no WHERE  sordsub.d1 >= {filter.DeliveryDateStart.Value.ToString("yyyyMMdd")} AND sordsub.d1 <= {filter.DeliveryDateEnd.Value.ToString("yyyyMMdd")} AND purcsub.sord_sn = sordsub.sn AND sordsub.d1 >= purcsub.trn_date group by sord.trn_no,sordsub.sn,fact.factnm2 ,sordsub.item_no,sordsub.quantity,sord.trn_date ,sordsub.d1,sordsub.NAME ) a";
                    ConcurrentDictionary<string, object?> dic = new ConcurrentDictionary<string, object?>();
                    result = 　await _supplierScoreRepository.GetBySql(sql, null);
                }
                catch (System.Exception ex)
                {
                    await _unitOfWork.RollbackAsync();
                    throw ex;
                }
                finally
                {
                    await _unitOfWork.DisposeAsync();
                }
            }
            return result;
        }

        private static List<MSupplierScoreDetail> GetSupplierDeliveryRateDetailByApi(int companyID, MSupplierScoreParameter filter, MCompanyDataSource dataSource)
        {
            List<MSupplierScoreDetail> result;
            var _apiSource = Utility.FromJson<MApiSource>(dataSource.apiSource);
            MApiInfo apiInfo = _apiSource.GetSupplierDeliveryRateDetail;
            result = GetDataByAPIAsync<List<MSupplierScoreDetail>>(apiInfo, companyID, filter).Result;
            return result;
        }
        /// <summary>
        /// 更新資料庫連線資訊並回傳資料來源
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="isTest">
        /// 是否為測試模式
        /// </param>
        /// <returns>
        /// 資料來源
        /// </returns>
        private async Task<MCompanyDataSource> UpdateDbInfoAsync(int companyID, bool isTest = true)
        {
            //取得公司資料來源
            MCompanyDataSource data_source = await _companyDataSourceService.GetDataSource(companyID);
            //更新資料來源
            DbConnectionInfo dbConnectionInfo = new DbConnectionInfo() { DbType = (int)VISDbType.mysql };
            if (isTest)
            {
                #region 從configuration取得資料庫連線資訊
                dbConnectionInfo = new DbConnectionInfo()
                {
                    //DbType = (int)VISDbType.mysql,
                    //ConnectionString = _configuration.GetConnectionString("MySqlConnectionString")
                    DbType = (int)VISDbType.mssql,
                    ConnectionString = _configuration.GetConnectionString("MsSqlConnectionString")
                };
                #endregion
            }
            else
            {
                #region 依據資料來源取得資料庫連線資訊
                dbConnectionInfo = Utility.GetDbInfoByDataSource(data_source);
                #endregion
            }
            //_supplierScoreRepository.UpdateConnectionInfo(dbConnectionInfo.ConnectionString, dbConnectionInfo.DbType);
            _unitOfWork.UpdateConnectionInfo(dbConnectionInfo.ConnectionString, dbConnectionInfo.DbType);

            return data_source;
        }
    }
}

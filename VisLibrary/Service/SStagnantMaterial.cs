using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Business.Interface;
using VisLibrary.Models;
using VisLibrary.Models.WHE;
using VisLibrary.Repositories.Interface;
using VisLibrary.Service.Interface;
using VisLibrary.UnitWork;
using VisLibrary.Utilities;
using static Mysqlx.Expect.Open.Types;
using static VisLibrary.Utilities.AllEnum;
using static VisLibrary.Utilities.ApiUtility;


namespace VisLibrary.Service
{
    public class SStagnantMaterial : ISStagnantMaterial
    {
        IRStagnantMaterial _rStagnantMaterial;
        IConfiguration _configuration;
        IRCompanyDataSource _repository;
        IUnitOfWork _unitOfWork;
        bool IsTest = true;
        ExceptionHandler _exceptionHandler;
        public SStagnantMaterial(ExceptionHandler exceptionHandler, IRStagnantMaterial rStagnantMaterial, IConfiguration configuration, IRCompanyDataSource repository,IUnitOfWork unitOfWork)
        {
            _exceptionHandler = exceptionHandler;
            _rStagnantMaterial = rStagnantMaterial;
            _configuration = configuration;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<MStagnantMaterial>> GetStagnantMaterialList(int CompanyID, MStagnantMaterialParameter? filter)
        {
            //更新資料庫連線資訊
            MCompanyDataSource _dataSource =
                _unitOfWork.RCompanyDataSource.GetByPk(CompanyID).Result;
            List<MStagnantMaterial> result = new List<MStagnantMaterial>();
            //判斷資料來源是否為api
            //if (_dataSource.sourceType != (int)VISDbType.api)
            //{
            //    //透過Repository取得資料
            //    _exceptionHandler.ExecuteWithExceptionHandling(() => result = _rStagnantMaterial.GetStagnantMaterialList(CompanyID, filter));
            //}
            if (_dataSource.sourceType == (int)VISDbType.api)
            {
               try
                {
                    //透過API取得資料
                    result = GetStagnantMaterialListByApi(CompanyID, filter, _dataSource);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"取得呆滯物料列表api異常:{ex.Message}");
                }
            }
            if (_dataSource.sourceType == (int)VISDbType.mssql)
            {
               try
                {
                    //透過API取得資料
                    result = await GetStagnantMaterialListBySqlCmd(CompanyID, filter, _dataSource);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"取得呆滯物料列表異常:{ex.Message}");
                    await _unitOfWork.RollbackAsync();
                }
                finally
                {
                   await _unitOfWork.DisposeAsync();
                }
            }
            return result;
        }
        /// <summary>
        /// 透過sqlCmd取得呆料統計表
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="filter"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task<List<MStagnantMaterial>> GetStagnantMaterialListBySqlCmd(int companyID, MStagnantMaterialParameter? filter, MCompanyDataSource dataSource)
        {
            //依照dataSource取得資料庫連線資訊
            DbConnectionInfo dbConnectionInfo = Utility.GetDbInfoByDataSource(dataSource);
            //更新資料庫連線資訊
            // _rStagnantMaterial.UpdateConnectionInfo(dbConnectionInfo);
            _unitOfWork.UpdateConnectionInfo(dbConnectionInfo.ConnectionString, dbConnectionInfo.DbType);
            // await _unitOfWork.InitializeAsync();
            await _unitOfWork.OpenAsyncConnection();
            //取得sql指令
            // 當前日期- 庫存天數=開始日期
            //string startDate ="20240101";
            string startDate = DateTime.Now.AddDays(0-filter.InventoryDays).ToString("yyyyMMdd");
            // string endDate = "20241231";
            // enddate = 當前月份的25號
            string endDate = DateTime.Now.ToString("yyyyMMdd");
           // string endDate =new DateTime(DateTime.Now.Year, DateTime.Now.Month, 25).ToString("yyyyMMdd");
            // 物料類別條件
            string class_condition = "";
            // 儲位條件
            string warehouse_condition = "";
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append( $"SELECT top(10000) isnull(b.LastMaterialWithdrawalDate,'---')  LastMaterialWithdrawalDate,a.ItemNumber,b.ItemName,b.WarehouseLocation,b.MaterialCategory,a.InventoryQuantity TotalRemainingInventory ,a.MaterialWithdrawalQuantity,a.SalesQuantity,(a.InventoryQuantity+a.MaterialWithdrawalQuantity+a.SalesQuantity-a.入庫數量) OpeningInventory,((a.MaterialWithdrawalQuantity+a.SalesQuantity)*100/(a.InventoryQuantity+a.MaterialWithdrawalQuantity+a.SalesQuantity-a.入庫數量))  MaterialWithdrawalRatio ,b.StockCost , b.StockCost * a.InventoryQuantity Subtotal,b.GridNumber FROM (SELECT item.item_no  ItemNumber, ISNULL(ITEM.PQTY_H, 0)  InventoryQuantity, ISNULL( (SELECT sum(ITEMIOS.QTY2) FROM itemios WHERE itemios.item_no=item.item_no AND itemios.io=1 AND itemios.trn_date> @startDate AND itemios.trn_date< @endDate GROUP BY itemios.item_no),0)  MaterialWithdrawalQuantity, ISNULL( (SELECT sum(ITEMIOS.QTY2) FROM itemios WHERE itemios.item_no=item.item_no AND itemios.io=2 AND itemios.trn_date> @startDate AND itemios.trn_date< @endDate GROUP BY itemios.item_no),0)  入庫數量, ISNULL( (SELECT sum(invosub.QUANTITY) FROM invosub WHERE invosub.item_no=item.item_no AND TYPE=1 AND invosub.trn_date> @startDate AND invosub.trn_date< @endDate GROUP BY invosub.item_no),0)  SalesQuantity FROM item LEFT JOIN ITEM_GD ON ITEM_GD.ITEM_NO = ITEM.ITEM_NO WHERE ITEM.PQTY_H>0  AND ITEM.COQTY = 0  AND ITEM.ISQTY = 0 AND ITEM.SMQTY >= 0 AND ITEM.SOQTY >= 0 AND ITEM_GD.QTY > 0 @cond3 )  a LEFT JOIN (SELECT DISTINCT ITEM.item_no  ItemNumber, ITEM.ITEMNM  ItemName, ITEM.PQTY_H  庫存數, ITEM.PLACE  WarehouseLocation, MAX(itemios.trn_date)  LastMaterialWithdrawalDate, ITEM_GD.GDNO  GridNumber , ITEM.SCOST  StockCost,CLASS.C_NAME  MaterialCategory FROM ITEM LEFT JOIN itemios ON itemios.ITEM_NO = ITEM.ITEM_NO LEFT JOIN ITEM_GD ON ITEM_GD.ITEM_NO = ITEM.ITEM_NO LEFT JOIN CLASS ON CLASS.CLASS = ITEM.CLASS WHERE ITEM_GD.QTY > 0 @cond2 @cond3  GROUP BY ITEM.ITEM_NO, ITEM.ITEMNM, ITEM.PQTY_H, ITEM.PLACE, ITEM_GD.GDNO, ITEM.SCOST, CLASS.C_NAME)  b ON a.ItemNumber = b.ItemNumber AND a.InventoryQuantity = b.庫存數 order by MaterialWithdrawalRatio ");
            if(filter.WarehouseLocation != null)
            {
                warehouse_condition = GetWareHouseCondition(filter, warehouse_condition);
            }
            if(filter.MaterialCategory !=null && filter.MaterialCategory.Count>0)
            {
                class_condition = GetClassCondition(filter, class_condition);
            }
            //從dataSource取得參數
            var sqlInfo = Utility.FromJson<SqlInfo>(dataSource.sqlcmd);
            // if sqlInfo.GetStagnantMaterialDetail is not null sqlcmd = sqlInfo.GetStagnantMaterialDetail
            if(sqlInfo.GetStagnantMaterialDetail != null)
            {
                sqlCmd = new StringBuilder(sqlInfo.GetStagnantMaterialDetail);
            }
            
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@startDate", startDate);
            parameters.Add("@endDate", endDate);

            //篩選條件2
            //keyValuePairs.TryAdd("@cond2", class_condition);
            // 篩選條件3
            //keyValuePairs.TryAdd("@cond3", $"And ({warehouse_condition})");
            if (warehouse_condition != "")
            {
                sqlCmd = sqlCmd.Replace("@cond3", $"And ({warehouse_condition})");
            }
            else
            {
                sqlCmd = sqlCmd.Replace("@cond3", "");
            }
            if(class_condition != "")
            {
                sqlCmd = sqlCmd.Replace("@cond2", $"And ({class_condition})");
            }
            else
            {
                sqlCmd = sqlCmd.Replace("@cond2", "");
            }
          
            //存放儲位
            // keyValuePairs.TryAdd("@warehouse", filter?.WarehouseLocation);
            //取得資料
            List<MStagnantMaterial> result = await _rStagnantMaterial.GetBySql(sqlCmd.ToString(), parameters, 0);
            return result;
        }

        private static string GetClassCondition(MStagnantMaterialParameter? filter, string class_condition)
        {
            string Total_type = "ITEM.CLASS";
            //if (Total_type != "")
            //{
            //    for (int i = 1; i < type.Items.Count; i++)
            //    {
            //        if (type.Items[i].Selected)
            //            condition_type += condition_type == "" ? $" {Total_type} = '{type.Items[i].Value}' " : $" OR {Total_type} = '{type.Items[i].Value}' ";
            //    }
            //    condition_type = condition_type != "" ? $" AND ({condition_type}) " : "";
            //}
            for (int i = 0; i < filter.MaterialCategory.Count; i++)
            {
                if (i == 0)
                {
                    class_condition += $"{Total_type} = '{filter.MaterialCategory[i]}'";
                }
                else
                {
                    class_condition += $" OR {Total_type} = '{filter.MaterialCategory[i]}'";

                }
            }

            return class_condition;
        }

        /// <summary>
        /// 取得儲位條件
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="warehouse_condition"></param>
        /// <returns></returns>
        private static string GetWareHouseCondition(MStagnantMaterialParameter? filter, string warehouse_condition)
        {
            string Total_warehouse = "ITEM_GD.GDNO";
            // 儲位篩選不為空
            // 加入儲位篩選sql語法
            for (int i = 0; filter.WarehouseLocation.Count > i; i++)
            {
                //if (warehouse.Items[i].Selected)
                //    condition += condition == "" ? $" {Total_warehouse} = '{warehouse.Items[i].Value}' " : $" OR {Total_warehouse} = '{warehouse.Items[i].Value}' ";
                if (i == 0)
                {
                    warehouse_condition += $"{Total_warehouse} = '{filter.WarehouseLocation[i]}'";
                }
                else
                {
                    warehouse_condition += $" OR {Total_warehouse} = '{filter.WarehouseLocation[i]}'";
                }
            }

            return warehouse_condition;
        }

        private static List<MStagnantMaterial> GetStagnantMaterialListByApi(int CompanyID, MStagnantMaterialParameter? filter, MCompanyDataSource _dataSource)
        {
            List<MStagnantMaterial> result;
            MApiSource? _apiSource = Utility.FromJson<MApiSource>(_dataSource.apiSource);
            MApiInfo mApiInfo = _apiSource.GetStagnantMaterialList;
            result = GetDataByAPIAsync<List<MStagnantMaterial>>(mApiInfo, CompanyID, filter).Result;
            return result;
        }
        /// <summary>
        /// 更新資料庫連線資訊, 並取得公司資料來源
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="isTest">
        /// 是否為測試模式
        /// </param>
        /// <returns>
        /// 公司資料來源
        /// </returns>
        private async Task<MCompanyDataSource> UpdateDbInfo(int companyID, bool isTest = false)
        {
            //取得公司資料來源
            MCompanyDataSource data_source = await _repository.GetByPk(companyID, "company_data_source");

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
          //  _unitOfWork.UpdateConnectionInfo(dbConnectionInfo.ConnectionString, dbConnectionInfo.DbType);
            return data_source;
        }
    }
}

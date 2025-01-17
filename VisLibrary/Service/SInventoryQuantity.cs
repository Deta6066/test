using VisLibrary.Models;

using VisLibrary.Models.WHE;
using VisLibrary.Repositories.Interface;
using Microsoft.Extensions.Configuration;
using static VisLibrary.Utilities.AllEnum;
using static VisLibrary.Utilities.ApiUtility;
using VisLibrary.Service.Interface;
using VisLibrary.Utilities;
using Newtonsoft.Json;
using System.Text;
using MySqlX.XDevAPI.Common;
using System.ComponentModel.Design;
using System.Collections.Concurrent;
using VisLibrary.UnitWork;

namespace VisLibrary.Service
{
    /// <summary>
    /// 庫存物料Service
    /// </summary>
    public class SInventoryQuantity : ISInventory
    {
        IUnitOfWork _unitOfWork;
        // IRInventory _inventoryRepository;
        IGenericRepositoryBase<MCompanyDataSource> _dataSource;
        IConfiguration _configuration;
        ExceptionHandler _exceptionHandler;
        public SInventoryQuantity(ExceptionHandler exceptionHandler, IRInventory inventoryDetail_repository, IRCompanyDataSource dataSource, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            // _inventoryRepository = inventoryDetail_repository;
            _dataSource = dataSource;
            _configuration = configuration;
            _exceptionHandler = exceptionHandler;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 取得物料庫存明細
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public async Task<List<MInventoryDetail>?> GetInventoryDetail(MInventoryParameter filter, int CompanyID)
        {
            //透過companyID取得資料來源
            //var _com = _dataSource.GetByPk(CompanyID, "company_data_source").Result;
            var _com = _unitOfWork.RCompanyDataSource.GetByPk(CompanyID).Result;
            List<MInventoryDetail>? list = new List<MInventoryDetail>();
            if (_com == null)
            {
                return null;
            }
            //判斷資料來源是否為API
            if (_com.sourceType == (int)VISDbType.api)
            {
                try
                {
                    //透過API取得資料
                    list = GetInventoryDetailByApi(filter, CompanyID, _com);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                //透過資料來源取得連線資訊
                #region 依照公司ID取得資料來源
                DbConnectionInfo dbInfo = Utility.GetDbInfoByDataSource(_com);
                #endregion
                #region local測試用
                //   DbConnectionInfo dbInfo = new DbConnectionInfo() { DbType = (int)VISDbType.mysql, ConnectionString = _configuration.GetConnectionString("MySqlConnectionString") };
                #endregion
                //_inventoryRepository.UpdateConnectionInfo(dbInfo);
                _unitOfWork.UpdateConnectionInfo(dbInfo.ConnectionString, dbInfo.DbType);
                try
                {
                    //  list = _inventoryRepository.GetInventoryDetail(filter, CompanyID);
                    list = await GetInventoryDetailBySqlCmd(_com, filter);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            return list;
        }
        /// <summary>
        /// 從API取得庫存明細
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="CompanyID"></param>
        /// <param name="_com"></param>
        /// <returns></returns>
        private static List<MInventoryDetail>? GetInventoryDetailByApi(MInventoryParameter filter, int CompanyID, MCompanyDataSource _com)
        {
            List<MInventoryDetail>? list;
            MApiSource? _apiSource = Utility.FromJson<MApiSource>(_com.apiSource);
            if (_apiSource != null)
            {
                MApiInfo mApiInfo = _apiSource.GetInventoryDetail;
                list = GetDataByAPIAsync<List<MInventoryDetail>>(mApiInfo, CompanyID, filter).Result;
                return list;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 從SQLCMD取得庫存明細
        /// </summary>
        /// <param name="dataSource">公司資料來源</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task<List<MInventoryDetail>> GetInventoryDetailBySqlCmd(MCompanyDataSource dataSource, MInventoryParameter filter)
        {
            // 從dataSource取得SQLCMD
            var sqlInfo = Utility.FromJson<SqlInfo>(dataSource.sqlcmd);
            // 條件篩選 倉位 類別
            string cond = "";
            if (filter.StorageLocationList.Count > 0)
            {
                cond += $" a.Warehouse in ({string.Join
                    (",", filter.StorageLocationList.Select(x => $"'{x}'"))})";
            }
            if (filter.CategoryList.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(cond))
                {
                    cond += " and";
                }
                cond += $" a.MaterialType in ({string.Join(",", filter.CategoryList.Select(x => $"'{x}'"))})";
            }
            //if cond 不為空 加上where
            if (!string.IsNullOrWhiteSpace(cond))
            {
                cond = " where " + cond;
            }
            string sql = $" select top(5000) a.* from ( SELECT item.item_no AS MaterialNo,itemnm AS MaterialName,CLASS.C_NAME AS MaterialType,CASE item.NOUSE WHEN 'Y' THEN '是' ELSE '' END IsStop,ITEM_GD.GDNO AS Warehouse,ITEM_GD.qty AS InventoryQuantity,SCOST AS InventoryAmount,(ITEM_GD.qty*SCOST) AS TotalInventoryAmount FROM item LEFT JOIN ITEM_GD ON ITEM_GD.ITEM_NO = ITEM.ITEM_NO  AND ITEM_GD.QTY >0 LEFT JOIN CLASS ON CLASS.CLASS = ITEM.CLASS WHERE PQTY_H>0) as a  {cond}";
            //sql = sql.Replace("@品號", "MaterialNo");
            //sql = sql.Replace("@品名規格", "MaterialName");
            //sql = sql.Replace("@物料類別", "MaterialType");
            //sql = sql.Replace("@是否停用", "IsStop");
            //sql = sql.Replace("@倉位", "Warehouse");
            //sql = sql.Replace("@庫存數量", "InventoryQuantity");
            //sql = sql.Replace("@庫存金額", "InventoryAmount");
            //sql = sql.Replace("@總庫存金額", "TotalInventoryAmount");
            //string sqlCmd = sqlInfo.GetInventoryDetail; 沒使用到?by秋註解
            //  var result = await _inventoryRepository.GetBySql(sql, prams);
            var result = await _unitOfWork.GetRepository<MInventoryDetail>().GetBySql(sql, null, 0);
            return result;
        }
    }
}

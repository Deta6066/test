using VisLibrary.Models;
using VisLibrary.Repositories.Interface;
using System.Collections.Generic;

using VisLibrary.Service;
using Microsoft.Extensions.Configuration;
using VisLibrary.Business.Interface;
using VisLibrary.Utilities;
using Service.Interface;
using static VisLibrary.Utilities.AllEnum;
using static VisLibrary.Utilities.ApiUtility;
using VisLibrary.Service.Interface;
using VisLibrary.Repositories;
using MySqlX.XDevAPI.Common;
using System.ComponentModel.Design;
using System.Data.Common;
using VisLibrary.Models.WHE;
using VisLibrary.UnitWork;

namespace VisLibrary.Service
{
    /// <summary>
    /// 供應商物料服務
    /// </summary>
    public class SSupplierMaterial : ISSupplierMaterial
    {
        IRSupplierMaterial _rSupplierMaterial;
        ISCompanyDataSource _companyDataSourceService;
        IConfiguration _configuration;
        ExceptionHandler _exceptionHandler;
        IUnitOfWork _unitOfWork;
        public SSupplierMaterial(ExceptionHandler exceptionHandler, IRSupplierMaterial rSupplierMaterial, IConfiguration configuration, ISCompanyDataSource companyDataSourceService, IUnitOfWork unitOfWork)
        {
            // _supplierMaterialService=new SSupplierMaterial(db);
            _rSupplierMaterial = rSupplierMaterial;
            _configuration = configuration;
            _companyDataSourceService = companyDataSourceService;
            _exceptionHandler = exceptionHandler;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 取得未交物件列表
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<MSupplierMaterialShortAge>> GetSupplierMaterialShortAgeAsync(int companyID, MSupplierMaterialParameter filter)
        {
            #region 依照公司ID取得資料來源
            //透過ID取得公司資料來源
            MCompanyDataSource companyDataSource = _companyDataSourceService.GetDataSource(companyID).Result;
            List<MSupplierMaterialShortAge> list = new List<MSupplierMaterialShortAge>();
            //透過資料來源取得DBInfo
            // DbConnectionInfo dbConnectionInfo = Utility.GetDbInfoByDataSource(companyDataSource);
            #endregion
            #region local測試用
            DbConnectionInfo dbConnectionInfo = new DbConnectionInfo() { DbType = (int)VISDbType.mssql, ConnectionString = GetConnectionString() };
            #endregion
            if (companyDataSource.sourceType != (int)VISDbType.api)
            {
                try
                {
                    //  _rSupplierMaterial.UpdateConnectionInfo(dbConnectionInfo);
                    _unitOfWork.UpdateConnectionInfo(dbConnectionInfo.ConnectionString, dbConnectionInfo.DbType);
                   // await _unitOfWork.InitializeAsync();
                    list = await _rSupplierMaterial.GetSupplierMaterialShortAge(companyID, filter);
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
            else
            {
                MApiSource? _apiSource = Utility.FromJson<MApiSource>(companyDataSource.apiSource);
                MApiInfo mApiInfo = _apiSource.GetSupplierMaterialShortAge;
                list = GetDataByAPIAsync<List<MSupplierMaterialShortAge>>(mApiInfo, companyID, filter).Result;
            }
            return list;
        }

        private string? GetConnectionString()
        {
            string connStr = "Data Source=192.168.1.210;Database=FJWSQL;User Id=DEK;Password=asus54886961;TrustServerCertificate=true;";
            return connStr;
           // return _configuration.GetConnectionString("MySqlConnectionString");
        }
    }
}

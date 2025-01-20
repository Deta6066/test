using VIS_API.Models;
using VIS_API.Service.Interface;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using static VIS_API.Utilities.AllEnum;
using DapperDataBase.Database.Interface;
using VIS_API.Repositories.Interface;
using VIS_API.UnitWork;

namespace VIS_API.Service
{
    public class SCompany : ISCompany
    {
        //private IGenericDb _db;
        const string TABLE_NAME = "company";
        private ISUser _userService;
        ISCompanyDataSource _companyDataSourceService;
        IConfiguration _configuration;
        IUnitOfWork _unitOfWork;
        public SCompany( IUnitOfWork unitOfWork, IConfiguration configuration, ISCompanyDataSource companyDataSource, ISUser sUser)
        {
            _configuration = configuration;
            //_db = factory.Create("Mysql");
            _companyDataSourceService = companyDataSource;
            _userService = sUser;
            _unitOfWork = unitOfWork;
        }
        
        /// <summary>
        /// 取得公司清單
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        public async Task<List<MCompany>> GetList(string cond)
        {
            await _unitOfWork.OpenAsyncConnection();
            List<MCompany> result = new();
            string sql = @"";
            if (string.IsNullOrEmpty(cond))
            {
                sql = $@"Select * from `{TABLE_NAME}`";
            }
            else
            {
                sql = $@"Select * from `{TABLE_NAME}` where {cond}";
            }
            return result;
        }
        /// <summary>
        /// 依使用者ID取得公司資料
        /// </summary>
        /// <param name="userID">使用者ID</param>
        /// <returns></returns>
        public async Task<MCompany> GetCompanyByUserID(int userID)
        {
            
            MCompany result = new MCompany();

            //if (userID > 0)
            //{
            //    //透過使用者ID取得使用者屬於哪間公司
            //    var user = await _userService.Get((uint)userID);
            //    int company_fk = user.company_fk;
            //    string sql = @$"Select * from `{TABLE_NAME}` where ID = {company_fk} ";
            //    //string sql = "";
            //    var comData = await _db.GetBySql(sql, null);
            //    result = comData.SingleOrDefault() ?? result;
            //}
            return result;
        }
        public async Task<MCompany> GetCompany(int companyID)
        {
            await _unitOfWork.OpenAsyncConnection();
            var company = _unitOfWork.GetRepository<MCompany>();
            MCompany result = new MCompany();
            string sql = @$"Select * from `{TABLE_NAME}` where ID ='{companyID}'";
            var comData = await company.GetBySql(sql, null);
            result = comData.SingleOrDefault() ?? result;
            //result.ID = 1;
            //result.companyName = "公司A";
            return result;
        }
    }
}

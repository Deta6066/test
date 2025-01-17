using VisLibrary.Models;

using VisLibrary.Utilities;
using VisLibrary.Service.Interface;
using DapperDataBase.Database.Interface;
using VisLibrary.Repositories.Interface;
using System.Collections.Concurrent;
using Dapper;
using VisLibrary.SqlGenerator;

namespace VisLibrary.Service
{
    /// <summary>
    /// 公司資料來源Service
    /// </summary>
    public class SCompanyDataSource(IGenericRepositoryBase<MCompanyDataSource> rcommon) : ISCompanyDataSource
    {
        const string SOURCE_TABLE_NAME = "company_data_source";
        IGenericRepositoryBase<MCompanyDataSource> _rCommon= rcommon;
        //public SCompanyDataSource(IMyDbFactory dbFactory)
        //{
        //    _dbFactory = dbFactory;
        //    _db = _dbFactory.Create("Mysql");
        //}
        public async Task<MCompanyDataSource> GetDataSource(int companyID)
        {
            MCompanyDataSource result = new MCompanyDataSource();
            string sql = @$"Select * from `{SOURCE_TABLE_NAME}` where company_fk ='{companyID}'";
            var source_data = await _rCommon.GetBySql(sql, null,0);
            if (source_data.Count > 0)
            {
                result = source_data[0];
            }
            return result;
        }
        /// <summary>
        /// 設定公司資料來源
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="data_source"></param>
        /// <returns></returns>
        public bool SetCompanyDataSource(int companyID, MCompanyDataSource data_source)
        {
            bool result = false;
            //檢查是否為首次設定
            //if 是，則新增資料
            //else 更新資料
            var source = GetDataSource(companyID);
            if (source.Result.ID == 0)
            {
                //新增
                // string sql = @$"Insert into `{SOURCE_TABLE_NAME}` (companyID,sourceType,jsonPath,dbParamater,apiUrl) values('{companyID}',{data_source.sourceType},'{data_source.sql}','{data_source.dbParamater}','{data_source.apiSource}')";
                string sql = @$"Insert into `{SOURCE_TABLE_NAME}` ({Utility.CmdColStr(data_source)}) values({Utility.CmdCValueStr(data_source)})";

                result = _rCommon.Insert(sql,null).Result > 0;
            }
            else
            {
                //更新
                //string sql = @$"Update `{SOURCE_TABLE_NAME}` set sourceType={data_source.sourceType},jsonPath='{data_source.sql}',dbParamater='{data_source.dbParamater}',apiUrl='{data_source.apiSource}' where company_fk='{companyID}'";
                string sql = @$"Update `{SOURCE_TABLE_NAME}` set sourceType=@sourceType,sqlcmd=@sqlcmd,dbParamater=@dbParamater,apiSource=@apiSource,dbType=@dbType where company_fk='{companyID}'";
                Dictionary<string, object?> param = new Dictionary<string, object?>
                {
                    { "@sourceType", data_source.sourceType },
                    { "@sqlcmd", data_source.sqlcmd },
                    { "@dbParamater", data_source.dbParamater },
                    { "@apiSource", data_source.apiSource },
                    { "@dbType", data_source.dbType }
                };

                //DynamicParameters? parameters =_rCommon.GenerateParameters(data_source, OperationType.Update);
                var parameters = new DynamicParameters();
                parameters.Add("@sourceType", data_source.sourceType);
                parameters.Add("@sqlcmd", data_source.sqlcmd);
                parameters.Add("@dbParamater", data_source.dbParamater);
                parameters.Add("@apiSource", data_source.apiSource);
                parameters.Add("@dbType", data_source.dbType);

                result = _rCommon.Update(sql, parameters).Result > 0;

            }
            return result;
        }
    }
}

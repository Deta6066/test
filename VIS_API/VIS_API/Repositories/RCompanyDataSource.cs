﻿using Dapper;
using DapperDataBase.Database.Interface;
using System.Collections.Concurrent;
using VIS_API.Models;
using VIS_API.Repositories.Base;
using VIS_API.Repositories.Interface;
using VIS_API.Service.Base;
using VIS_API.SqlGenerator;
using VIS_API.Utilities;

namespace VIS_API.Repositories
{
    public class RCompanyDataSource(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<MCompanyDataSource> sqlGenerator) : GenericRepository<MCompanyDataSource>(propertyProcessor, db, sqlGenerator), IRCompanyDataSource
    {
        const string TableName = "company_data_source";
        public  Task<int> Delete(int pk)
        {
            throw new NotImplementedException();
        }

        public Task<List<MCompanyDataSource>> GetAll()
        {
            throw new NotImplementedException();
        }


        public async  Task<MCompanyDataSource?> GetByPk(int? pk)
        {
            string sql = @$"select * from {TableName} where company_fk= @pk";
            var parameters = new DynamicParameters();
            parameters.Add("@pk", pk);

            return (await _db.GetListAsync<MCompanyDataSource>(sql, parameters)).FirstOrDefault();
        }

        public  Task<int> Insert(MCompanyDataSource obj, bool autoIncrement = true)
        {
            throw new NotImplementedException();
        }

        public  async Task<int> Update(MCompanyDataSource? obj)
        {
            string sql = $"UPDATE {TableName} SET dbType = @dbType WHERE company_fk = @pk";

            var parameters = new DynamicParameters();
            parameters.Add("@dbType", 99);
            parameters.Add("@pk", 2);

            return await base.Update(sql, parameters);
        }
    }
}

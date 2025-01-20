using Dapper;
using DapperDataBase.Database.Interface;
using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VIS_API.Models;
using VIS_API.Models.WHE;
using VIS_API.Repositories.Base;
using VIS_API.Repositories.Interface;
using VIS_API.Service.Base;
using VIS_API.SqlGenerator;
using VIS_API.Utilities;

namespace VIS_API.Repositories
{
    public class RErrorLog : GenericRepository<ErrorLog> ,IErrorLogRepository
    {
        public RErrorLog(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<ErrorLog> sqlGenerator)
     : base(propertyProcessor, db, sqlGenerator)
        {
        }

        public  Task<int> Delete(int pk)
        {
            throw new NotImplementedException();
        }

        public  Task<List<ErrorLog>> GetAll()
        {
            throw new NotImplementedException();
        }

        public  Task<ErrorLog?> GetByPk(int? pk)
        {
            throw new NotImplementedException();
        }

        public  Task<int> Insert(ErrorLog obj, bool autoIncrement = true)
        {
            throw new NotImplementedException();
        }

        public  async Task LogErrorAsync(ErrorLog errorLog)
        {
            var sql = "INSERT INTO ErrorLog (Message, StackTrace, Timestamp, SourceIp) VALUES (@Message, @StackTrace, @Timestamp, @SourceIp)";
            
            var parameters = new DynamicParameters();
            parameters.Add("@Message", errorLog.Message);
            parameters.Add("@StackTrace", errorLog.StackTrace);
            parameters.Add("@Timestamp", errorLog.Timestamp);
            parameters.Add("@SourceIp", errorLog.SourceIp);

            await _db.ExecuteNonQueryAsync(sql, parameters);
        }

        public  Task<int> Update(ErrorLog obj)
        {
            throw new NotImplementedException();
        }
    }
}

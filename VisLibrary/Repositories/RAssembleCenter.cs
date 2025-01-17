using DapperDataBase.Database.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisLibrary.Models;
using VisLibrary.Repositories.Base;
using VisLibrary.Repositories.Interface;
using VisLibrary.Service.Base;
using VisLibrary.SqlGenerator;
using VisLibrary.Utilities;

namespace VisLibrary.Repositories
{
    public class RAssembleCenter : GenericRepositoryBase<MAssembleCenter>, IRAssembleCenter
    {
        public RAssembleCenter(
            IPropertyProcessor propertyProcessor,
            IGenericDb db,
            ISqlGenerator<MAssembleCenter> sqlGenerator
        ) : base(propertyProcessor, db, sqlGenerator)
        {
            // 如有需要，可在此進行額外初始化
        }

        public async Task<List<MAssembleCenter>> GetByPkSS(string? pk)
        {
            string sql = @$"SELECT * from assemblecenter where company_fk ='2'";
            Dictionary<string, object?>? properties = new Dictionary<string, object?>();
            var result = await _db.GetListAsync<MAssembleCenter>(sql, null);
            result = _propertyProcessor.ProcessPropertiesForList(result);
            return (result);
        }
        
    }
}

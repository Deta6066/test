using DapperDataBase.Database.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VIS_API.Models;
using VIS_API.Repositories.Base;
using VIS_API.Repositories.Interface;
using VIS_API.Service.Base;
using VIS_API.SqlGenerator;
using VIS_API.Utilities;
using static VIS_API.Utilities.AllEnum;

namespace VIS_API.Repositories
{
    public class GenericRepository<T>(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<T> sqlGenerator) : GenericRepositoryBase<T>(propertyProcessor, db, sqlGenerator) where T : class
    {
        //private readonly ISqlGenerator<T> _sqlGenerator = sqlGenerator;

       

        

       
       

        //public override Task<T?> GetByPk(int? pk, string tableName)
        //{
        //    throw new NotImplementedException();
        //}

      

        
      

        
       
    }
}

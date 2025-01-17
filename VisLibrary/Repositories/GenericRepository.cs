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
using static VisLibrary.Utilities.AllEnum;

namespace VisLibrary.Repositories
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

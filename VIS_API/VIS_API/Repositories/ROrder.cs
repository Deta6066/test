using VIS_API.Models;
using VIS_API.Models.ViewModel;
using System.Collections.Generic;

using VIS_API.Repositories.Interface;
using static VIS_API.Utilities.AllEnum;
using Dapper;
using VIS_API.Service.Base;
using VIS_API.Repositories.Base;
using VIS_API.Utilities;
using DapperDataBase.Database.Interface;
using VIS_API.SqlGenerator;

namespace VIS_API.Repositories
{
    public class ROrder(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<Order> sqlGenerator) : GenericRepositoryBase<Order>(propertyProcessor, db, sqlGenerator), IROrder
    {
        
       
    }
}

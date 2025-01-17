using VisLibrary.Models;
using VisLibrary.Models.ViewModel;
using System.Collections.Generic;

using VisLibrary.Repositories.Interface;
using static VisLibrary.Utilities.AllEnum;
using Microsoft.Data.SqlClient;
using Dapper;
using VisLibrary.Service.Base;
using VisLibrary.Repositories.Base;
using VisLibrary.Utilities;
using DapperDataBase.Database.Interface;
using VisLibrary.SqlGenerator;

namespace VisLibrary.Repositories
{
    public class ROrder(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<Order> sqlGenerator) : GenericRepositoryBase<Order>(propertyProcessor, db, sqlGenerator), IROrder
    {
        
       
    }
}

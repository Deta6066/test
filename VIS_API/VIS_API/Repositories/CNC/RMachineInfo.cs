using Dapper;
using DapperDataBase.Database.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Models;
using VIS_API.Models.CNC;
using VIS_API.Repositories.Base;
using VIS_API.Repositories.Interface;
using VIS_API.Service.Base;
using VIS_API.SqlGenerator;

namespace VIS_API.Repositories.CNC
{
    public class RMachineInfo(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<MMachineInfo> sqlGenerator) : GenericRepositoryBase<MMachineInfo>(propertyProcessor, db, sqlGenerator), IRMachineInfo
    {
        
      

        
    }
}

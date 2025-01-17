using Dapper;
using DapperDataBase.Database.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models;
using VisLibrary.Models.CNC;
using VisLibrary.Repositories.Base;
using VisLibrary.Repositories.Interface;
using VisLibrary.Service.Base;
using VisLibrary.SqlGenerator;

namespace VisLibrary.Repositories.CNC
{
    public class RMachineInfo(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<MMachineInfo> sqlGenerator) : GenericRepositoryBase<MMachineInfo>(propertyProcessor, db, sqlGenerator), IRMachineInfo
    {
        
      

        
    }
}

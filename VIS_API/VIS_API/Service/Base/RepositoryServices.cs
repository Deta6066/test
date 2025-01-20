using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Repositories.Interface;
using VIS_API.Utilities;

namespace VIS_API.Service.Base
{
    public class RepositoryServices_UnUsed
    {
        public IPropertyProcessor PropertyProcessor { get; }
        public IDbConnectionManager DbConnectionManager { get; }
        public ExceptionHandler ExceptionHandler { get; }

        public RepositoryServices_UnUsed(
            IPropertyProcessor propertyProcessor,
            IDbConnectionManager dbConnectionManager,
            ExceptionHandler exceptionHandler)
        {
            PropertyProcessor = propertyProcessor;
            DbConnectionManager = dbConnectionManager;
            ExceptionHandler = exceptionHandler;
        }
    }
}

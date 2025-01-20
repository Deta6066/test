using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Models;
using VIS_API.Service.Base;
using VIS_API.Service.Interface;
using VIS_API.UnitWork;
using VIS_API.Utilities;
using static VIS_API.Utilities.AllEnum;

namespace VIS_API.Service
{

    public class SAssembleCenter: ServiceBase, IAssembleCenter
    {
        private readonly ILogger<SOrder> _logger;
        private readonly IUnitOfWork _unitOfWork;


        public SAssembleCenter(ExceptionHandler exceptionHandler, IServiceProvider serviceProvider, ILogger<SOrder> logger, IOptions<SqlCmdConfig> sqlCmdConfig, IUnitOfWork unitOfWork)
            : base(sqlCmdConfig)
        {
           
            _logger = logger;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
            public async Task<List<MAssembleCenter>?> GetList(CompanyParameter parameter)
        {
            await _unitOfWork.OpenAsyncConnection();
            var s = await _unitOfWork._assembleCenter.GetByPkSS("2");
            var assemble = _unitOfWork.GetRepository<MAssembleCenter>();
            var data = await assemble.GetAll("assemblecenter");
            var filteredData = data.Where(x => x.Company_fk == parameter.companyId).ToList();
            if (filteredData == null || filteredData.Count == 0)
            {
                return null;
            }

            return filteredData;
        }
    }
}

using VIS_API.Models;
using VIS_API.Utilities;

using DapperDataBase.Database.Interface;
using VIS_API.Repositories.Interface;
using VIS_API.Repositories;
using VIS_API.Service.Interface;
using VIS_API.UnitWork;
using System.ComponentModel.Design;

namespace VIS_API.Service
{
    public class SRole:ISRole
    {
        private readonly IRRole _roleService;
        private readonly IUnitOfWork _unitOfWork;

        public SRole(IRRole repository, IUnitOfWork unitOfWork)
        {
            _roleService = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MRole?> Get(int pk)
        {
          
            var result = await _roleService.GetByPk(pk);

            return result;
        }
    }
}
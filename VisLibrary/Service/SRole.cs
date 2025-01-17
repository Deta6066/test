using VisLibrary.Models;
using VisLibrary.Utilities;

using DapperDataBase.Database.Interface;
using VisLibrary.Repositories.Interface;
using VisLibrary.Repositories;
using VisLibrary.Service.Interface;
using VisLibrary.UnitWork;
using System.ComponentModel.Design;

namespace VisLibrary.Service
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
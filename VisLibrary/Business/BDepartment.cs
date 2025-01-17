using VisLibrary.Service;
using VisLibrary.Utilities;

using DapperDataBase.Database.Interface;

namespace VisLibrary.Business
{
    public class BDepartment
    {
        private readonly Sdepartment _departmentService;

        public BDepartment(IMySqlDb db)
        {
            _departmentService = new(db);
        }

        public async Task<Models.department?> Get(uint pk)
        {
            return await _departmentService.Get(pk);
        }

        public async Task<List<Models.department>> GetList()
        {
            return await _departmentService.GetList();
        }

        public async Task Set(Models.department obj)
        {
            if (obj.pk == 0)
                obj.pk = await _departmentService.Insert(obj);
            else
                _ = await _departmentService.Update(obj);
        }

        public async Task Delete(uint pk)
        {
            _ = await _departmentService.Delete(pk);
        }
    }
}
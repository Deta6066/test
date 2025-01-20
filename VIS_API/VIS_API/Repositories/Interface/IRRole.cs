using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Models;

namespace VIS_API.Repositories.Interface
{
    public interface IRRole: IGenericRepositoryBase<MRole>
    {
        public Task<string?> GetAccess(uint pk);
        public Task<MRole?> GetByPk(int pk);
        public Task<int> Insert(MRole obj, bool autoIncrement = true);
        public Task<int> Update(MRole obj);
        public Task<int> Delete(int pk);
        
    }
}

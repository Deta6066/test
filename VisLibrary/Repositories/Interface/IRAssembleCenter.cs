using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models;

namespace VisLibrary.Repositories.Interface
{
    public interface IRAssembleCenter: IGenericRepositoryBase<MAssembleCenter>
    {
        Task<List<MAssembleCenter>> GetByPkSS(string? pk);

    }
}

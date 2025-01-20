using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Models;
using VIS_API.Repositories;

namespace VIS_API.Service.Interface
{
    public interface ISRole
    {
        Task<MRole?> Get(int pk);

    }
}

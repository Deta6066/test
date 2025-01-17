using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models;
using VisLibrary.Repositories;

namespace VisLibrary.Service.Interface
{
    public interface ISRole
    {
        Task<MRole?> Get(int pk);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models;

namespace VisLibrary.Service.Interface
{
    public interface ISMenu
    {
        Task<MMenu?> Get(int pk);
        Task<List<MMenu>> GetList();
    }
}

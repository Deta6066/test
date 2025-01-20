using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Models;

namespace VIS_API.Service.Interface
{
    public interface ISCompany
    {
        Task<MCompany> GetCompany(int userId);
       // Task<MCompanyDataSource> GetCompany_Data_Source(int companyID);
    }
}

using VIS_API.Models;

namespace VIS_API.Service.Interface
{
    public interface ISCompanyDataSource
    {
        Task<MCompanyDataSource> GetDataSource(int companyID);
        bool SetCompanyDataSource(int companyID, MCompanyDataSource data_source);
    }
}
using VisLibrary.Models;

namespace VisLibrary.Service.Interface
{
    public interface ISCompanyDataSource
    {
        Task<MCompanyDataSource> GetDataSource(int companyID);
        bool SetCompanyDataSource(int companyID, MCompanyDataSource data_source);
    }
}
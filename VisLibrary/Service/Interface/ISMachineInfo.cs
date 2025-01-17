using VisLibrary.Models;
using VisLibrary.Models.CNC;

namespace VisLibrary.Service.Interface
{
    /// <summary>
    /// 機台資訊Service
    /// </summary>
    public interface ISMachineInfo
    {
        Task<List<MMachineInfo>> GetMachineInfo(MCompanyDataSource companyDataSource, MMachineInfoParameter parameter);
    }
}
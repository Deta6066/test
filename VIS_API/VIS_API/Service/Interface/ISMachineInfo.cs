using VIS_API.Models;
using VIS_API.Models.CNC;

namespace VIS_API.Service.Interface
{
    /// <summary>
    /// 機台資訊Service
    /// </summary>
    public interface ISMachineInfo
    {
        Task<List<MMachineInfo>> GetMachineInfo(MCompanyDataSource companyDataSource, MMachineInfoParameter parameter);
    }
}
using VIS_API.Models.CNC;

namespace VIS_API.Service.Interface
{
    public interface ISAps_info
    {
        /// <summary>
        /// 取得可視化資訊
        /// </summary>
        /// <param name="companyID">公司ID</param>
        /// <returns></returns>
        List<MAps_info> GetAps_infoList(int companyID = 0);
    }
}
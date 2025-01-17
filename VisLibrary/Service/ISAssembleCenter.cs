using VisLibrary.Models;

namespace VisLibrary.Service
{
    public interface ISAssembleCenter
    {
        /// <summary>
        /// 取得加工中心清單
        /// </summary>
        /// <param name="CompanyID">公司ID</param>
        /// <returns></returns>
        Task<List<MAssembleCenter>> GetAssemblecenter(int CompanyID);
    }
}
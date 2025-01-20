using NLog.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Models.PCD;

namespace VIS_API.Service.Interface
{
    /// <summary>
    /// 歷史用量統計服務
    /// </summary>
    public interface ISHistoricalMaterialUsage
    {
        /// <summary>
        /// 取得歷史用料統計表
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
       Task<List<HistoricalMaterialUsageModel>> GetHistoricalMaterials(MHistoricalMaterialUsageParameter filter, int companyID);
        /// <summary>
        /// 取得領料紀錄
        /// </summary>
        /// <param name="MHistoricalMaterialUsageParameter"></param>
        /// 
        /// <returns></returns>
       Task<List<HistoricalMaterialUsageModel_Detail>> GetHistoricalMaterialUsageModel_Detail(MHistoricalMaterialUsageParameter filter);
    }
}

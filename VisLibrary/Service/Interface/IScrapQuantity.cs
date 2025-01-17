using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models.WHE;

namespace VisLibrary.Service.Interface
{
    /// <summary>
    /// 報廢數量統計服務介面
    /// </summary>
    public interface IScrapQuantity
    {
        /// <summary>
        /// 刪除報廢資料明細表
        /// </summary>
        /// <param name="scrapQuantityID">報廢明細ID</param>
        /// <returns></returns>
        Task<int> DeleteScrapQuantity(int companyID, int scrapQuantityID);

        /// <summary>
        /// 取得報廢資料明細表
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<List<MScrapQuantity>> GetScrapQuantityList(int companyID, MScrapQuantityParameter? filter);
        /// <summary>
        /// 更新報廢數量
        /// </summary>
        /// <param name="scrapQuantity"></param>
        /// <returns></returns>
        Task<int> UpdateScrapQuantity(int companyID, MScrapQuantity scrapQuantity);
    }
}

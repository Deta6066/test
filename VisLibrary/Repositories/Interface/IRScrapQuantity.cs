using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models.WHE;

namespace VisLibrary.Repositories.Interface
{
    /// <summary>
    /// 報廢數量統計資料存取介面
    /// </summary>
    public interface IRScrapQuantity: IGenericRepositoryBase<MScrapQuantity>

    {
        /// <summary>
        /// 取得報廢資料明細表
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<MScrapQuantity> GetScrapQuantityList(int companyID, MScrapQuantityParameter filter);
    }
}

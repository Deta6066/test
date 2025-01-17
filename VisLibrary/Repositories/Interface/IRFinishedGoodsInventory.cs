using VisLibrary.Models;
using VisLibrary.Models.WHE;

namespace VisLibrary.Repositories.Interface
{
    /// <summary>
    /// 成品庫存Repository介面
    /// </summary>
    public interface IRFinishedGoodsInventory: IGenericRepositoryBase<MFinishGoodDetail>

    //IGenericRepositoryBase<HistoricalMaterialUsageModel> 
    {
        /// <summary>
        /// 取得成品庫存明細
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="companyID">公司ID</param>
        /// <param name="dataSource">公司資料來源</param>
        /// <returns></returns>
        public Task<string> GetFinishedGoodsInventoryDetailSql(MInventoryParameter filter, int companyID,MCompanyDataSource? dataSource=null);
    }
}

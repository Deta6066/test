using VIS_API.Models;

namespace VIS_API.Repositories.Interface
{
    /// <summary>
    ///  供應商物料資料存取介面
    /// </summary>
    public interface IRSupplierMaterial: IGenericRepositoryBase<MSupplierMaterialShortAge>
    {
        /// <summary>
        /// 取得供應商未交物料
        /// </summary>
        /// <param name="companyID">公司ID</param>
        /// <returns></returns>
        public Task<List<MSupplierMaterialShortAge>> GetSupplierMaterialShortAge(int companyID, MSupplierMaterialParameter filter);
    }
}

using VIS_API.Models;

namespace Service.Interface
{
    /// <summary>
    /// 供應商物料服務介面
    /// </summary>
    public interface ISSupplierMaterial
    {
        /// <summary>
        /// 取得未交物件列表
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
       Task<List<MSupplierMaterialShortAge>> GetSupplierMaterialShortAgeAsync(int companyID, MSupplierMaterialParameter filter);
    }
}

using VIS_API.Models.WHE;

namespace VIS_API.Repositories.Interface
{
    /// <summary>
    /// 呆滯物料資料存取介面
    /// </summary>
    public interface IRStagnantMaterial: IGenericRepositoryBase<MStagnantMaterial>
    {
        /// <summary>
        /// 取得呆滯物料統計表
        /// </summary>
        /// <param name="CompanyID">公司ID</param>
        /// <returns></returns>
        List<MStagnantMaterial> GetStagnantMaterialList(int CompanyID, MStagnantMaterialParameter? filter);
    }
}

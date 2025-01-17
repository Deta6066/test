using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models.WHE;

namespace VisLibrary.Service.Interface
{
    /// <summary>
    /// 呆滯物料業務邏輯
    /// </summary>
    public interface ISStagnantMaterial
    {
        /// <summary>
        /// 取得呆滯物料統計表
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public Task<List<MStagnantMaterial>> GetStagnantMaterialList(int CompanyID, MStagnantMaterialParameter? filter);
    }
}

using Microsoft.AspNetCore.Mvc;
using VIS_API.Models;
using VIS_API.Models.WHE;
using VIS_API.Service.Interface;

namespace VIS_API.Controllers.WHE
{
    /// <summary>
    /// 呆滯物料控制器
    /// </summary>
    [ApiController]
    public class StagnantMaterialController : Controller
    {
        ISStagnantMaterial _SStagnantMaterial;
        public StagnantMaterialController(ISStagnantMaterial sStagnantMaterial)
        {
            _SStagnantMaterial = sStagnantMaterial;
        }
        /// <summary>
        /// 取得呆滯物料統計表
        /// </summary>
        /// <param name="filter">呆料篩選器</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[Controller]/GetStagnantMaterialList")]
        public async Task<ApiResponse<VMStagnantMaterial>> GetStagnantMaterialList([FromBody] MStagnantMaterialParameter filter)
        {
            List<MStagnantMaterial> result = new List<MStagnantMaterial>();
            //透過Business取得呆滯物料統計表
            try
            {
                result = await _SStagnantMaterial.GetStagnantMaterialList(filter.CompanyID, filter);
                if (result == null)
                {
                    return new ApiResponse<VMStagnantMaterial>(new VMStagnantMaterial(), success: false, ResponseStatusCode.NoContent, "null");
                }
                VMStagnantMaterial vmStagnantMaterial = new VMStagnantMaterial();
                vmStagnantMaterial.MStagnantMaterialList = result;
                return new ApiResponse<VMStagnantMaterial>(vmStagnantMaterial, success: true);
                // return vmStagnantMaterial;
            }
            catch (Exception ex)
            {
                return new ApiResponse<VMStagnantMaterial>(new VMStagnantMaterial(), success: false, ResponseStatusCode.NotFound, ex.Message);
            }
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using VIS_API.Models;
using VIS_API.Models.View;

namespace VIS_API.Controllers.PCD
{
    /// <summary>
    /// 供應商催料控制器
    /// </summary>
    [ApiController]
    public class SupplierMaterialReminderController : Controller
    {
        // IGenericDb _db;
        ISSupplierMaterial supplierMaterialService;
        public SupplierMaterialReminderController(ISSupplierMaterial _SSupplierMaterial)
        {
            // _db = db;
            supplierMaterialService = _SSupplierMaterial;
        }
        /// <summary>
        /// 取得供應商未交物料
        /// </summary>
        /// <param name="filter">資料篩選器</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[Controller]/GetSupplierMaterialShortAge")]
        public async Task<ApiResponse<VMSupplierMaterialShortAge>> GetSupplierMaterialShortAge([FromBody] MSupplierMaterialParameter? filter)
        {
            VMSupplierMaterialShortAge vMSupplierMaterialShortAge = new VMSupplierMaterialShortAge();
            try
            {
                //透過篩選器取得供應商未交物料
                List<MSupplierMaterialShortAge> supplierMaterialShortAgeModels = await supplierMaterialService.GetSupplierMaterialShortAgeAsync(filter.CompanyID, filter);
                vMSupplierMaterialShortAge.SupplierScoreDetailList = supplierMaterialShortAgeModels;
                if (supplierMaterialShortAgeModels == null)
                {
                    return new ApiResponse<VMSupplierMaterialShortAge>(new VMSupplierMaterialShortAge(), success: false, ResponseStatusCode.NoContent, "null");
                }
                return new ApiResponse<VMSupplierMaterialShortAge>(vMSupplierMaterialShortAge, success: true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<VMSupplierMaterialShortAge>(new VMSupplierMaterialShortAge(), success: false, ResponseStatusCode.NotFound, ex.Message);
            }

        }
    }
}

using Microsoft.AspNetCore.Mvc;
using VisLibrary.Models;
using VisLibrary.Service.Interface;

namespace VIS_API.Controllers
{
    /// <summary>
    /// 公司資料控制器
    /// </summary>
    [ApiController]
    public class CompanyController : Controller
    {
        ISCompany _companyService;
        public CompanyController(ISCompany companyService)
        {
            _companyService = companyService;
        }
        //取得公司資料
        [HttpPost]
        [Route("[Controller]/GetCompany")]
        public async Task<MCompany> GetCompanyAsync([FromBody] CompanyID_data companyID_Data)
        {
            MCompany company = await _companyService.GetCompany(companyID_Data.companyID);
            return company;
        }
    }
}

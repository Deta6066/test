using Microsoft.AspNetCore.Mvc;
using VisLibrary.Models;
using VisLibrary.Service.Interface;

namespace VIS_API.Controllers
{
    /// <summary>
    /// 公司資料來源控制器
    /// </summary>
    [ApiController]
    public class CompanyDataSourceController : Controller
    {
        ISCompanyDataSource _companyDataSourceService;
        public CompanyDataSourceController(ISCompanyDataSource companyDataSourceService)
        {
            _companyDataSourceService = companyDataSourceService;
        }
        /// <summary>
        /// 取得公司資料來源
        /// </summary>
        /// <param name="companyID_Data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[Controller]/GetCompanyDataSource")]
        public async Task<MCompanyDataSource> GetCompanyDataSourceAsync([FromBody] CompanyID_data companyID_Data)
        {
            MCompanyDataSource source = await _companyDataSourceService.GetDataSource(companyID_Data.companyID);
            return source;
        }
        /// <summary>
        /// 設定公司資料來源
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[Controller]/SetCompanyDataSource")]
        public bool SetCompanyDataSourceAsync([FromHeader] int companyID, [FromBody] MCompanyDataSource source)
        {
            bool result = _companyDataSourceService.SetCompanyDataSource(source.company_fk, source);
            return result;
        }

    }
    public class CompanyID_data
    {
        public int companyID { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using VisLibrary.Models;
using VisLibrary.Models.SLS;
using VisLibrary.Models.View;
using VisLibrary.Service.Interface;

namespace VIS_API.Controllers.SLS
{
    /// <summary>
    /// 未交易客戶控制器
    /// </summary>
    [ApiController]
    public class InactiveCustomersController : Controller
    {
        ISInactiveCustomer _sInactiveCustomer;
        public InactiveCustomersController(ISInactiveCustomer sInactiveCustomer)
        {
            _sInactiveCustomer = sInactiveCustomer;
        }
        /// <summary>
        /// 取得未交易客戶
        /// </summary>
        /// <param name="paramater"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[Controller]/GetMInactiveCustomers")]
        public async Task<ApiResponse<VMInactiveCustomer>> GetMInactiveCustomers([FromBody] MInactiveCustomerParamater paramater)
        {
            VMInactiveCustomer vmInactiveCustomer = new VMInactiveCustomer();
            List<MInactiveCustomer> list = new List<MInactiveCustomer>();
            try
            {
                list = await _sInactiveCustomer.GetMInactiveCustomers(paramater);
                if(list == null)
                {
                    return new ApiResponse<VMInactiveCustomer>(new VMInactiveCustomer(), success: false,ResponseStatusCode.NoContent,"null");
                }
                vmInactiveCustomer.MInactiveCustomerList = list;
                return new ApiResponse<VMInactiveCustomer>(vmInactiveCustomer, success: true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<VMInactiveCustomer>(new VMInactiveCustomer(), success: false, ResponseStatusCode.NotFound, ex.Message);
            }
           
        }
    }
}

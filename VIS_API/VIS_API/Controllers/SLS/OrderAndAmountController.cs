using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VisLibrary.Models;
using VisLibrary.Models.API;
using VisLibrary.Models.View;
using VisLibrary.Service.Interface;
using VisLibrary.Utilities;

namespace VIS_API.Controllers.SLS
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrderAndAmountController : ControllerBase
    {
        private readonly IOrder _orderService;
        private readonly ILogger<OrderAndAmountController> _logger;
        private readonly ExceptionHandler _exceptionHandler ;

        public OrderAndAmountController(IOrder orderService, ILogger<OrderAndAmountController> logger, ExceptionHandler exceptionHandler)
        {
            _orderService = orderService;
            _logger = logger;
            _exceptionHandler = exceptionHandler;
        }
        [HttpPost("GetOrderList")]
        public async Task<IActionResult> GetList([FromBody] OrderParameter parameters)
        {
            try
            {
                var companyFk = int.Parse(User.FindFirst("companyFk").Value);
                parameters.Company_fk = companyFk;
                var data = await _orderService.GetList(parameters);
                if (data == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, new
                    {
                        code = "204",
                        message = "請求成功但無資料",
                        data = new VOrder()
                    });
                }
                return Ok(data);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "GetOrderList");

                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    code = "403",
                    message = "抓取資料異常",
                    data = new VOrder()
                });
            }

            //return data;
        }
    }
}

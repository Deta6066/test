using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VIS_API.Controllers.SLS;
using VIS_API.Models;
using VIS_API.Service.Interface;
using VIS_API.UnitWork;

namespace VIS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyConfigurationController : ControllerBase
    {
        private readonly IOrder _orderService;
        private readonly ILogger<OrderAndAmountController> _logger;
        //private readonly ITransactionManager _transactionManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;

        public CompanyConfigurationController(IUnitOfWork unitOfWork, IOrder orderService, ILogger<OrderAndAmountController> logger, IServiceProvider serviceProvider)
        {
            _orderService = orderService;
            _logger = logger;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _serviceProvider = serviceProvider;

        }


        [HttpPost("GetDateRange")]
        public async Task<IActionResult> GetDateRange()
        {
            try
            {
                var companyFk = int.Parse(User.FindFirst("companyFk").Value);

                await _unitOfWork.InitializeAsync(); // 開始交易

                var companySettingRepository = _unitOfWork.GetRepository<MCompanyConfiguration>();
                var result = await companySettingRepository.GetByPk(companyFk, "company_set");

                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                int currentDay = DateTime.Now.Day;

                DateTime startDate;
                DateTime endDate;

                if (result == null || !result.start_day.HasValue || !result.end_day.HasValue)
                {
                    // 無參數設定，當月1號至當月最後一天 23:59:59
                    startDate = new DateTime(currentYear, currentMonth, 1, 0, 0, 0);
                    // 當月最後一天
                    var lastOfMonth = startDate.AddMonths(1).AddDays(-1);
                    endDate = new DateTime(lastOfMonth.Year, lastOfMonth.Month, lastOfMonth.Day, 23, 59, 59);
                }
                else
                {
                    int startDay = result.start_day.Value;
                    int endDay = result.end_day.Value;

                    if (startDay > currentDay)
                    {
                        // start_day > today, 則往前一個月的 start_day
                        var prevMonth = new DateTime(currentYear, currentMonth, 1).AddMonths(-1);
                        startDate = new DateTime(prevMonth.Year, prevMonth.Month, startDay, 0, 0, 0);

                        // 結束日以現在月份的 end_day 作結 (23:59:59)
                        endDate = new DateTime(currentYear, currentMonth, endDay, 23, 59, 59);
                    }
                    else
                    {
                        // start_day <= today
                        startDate = new DateTime(currentYear, currentMonth, startDay, 0, 0, 0);

                        if (endDay < startDay)
                        {
                            // 跨月，end_day 在下個月
                            var nextMonth = new DateTime(currentYear, currentMonth, 1).AddMonths(1);
                            endDate = new DateTime(nextMonth.Year, nextMonth.Month, endDay, 23, 59, 59);
                        }
                        else
                        {
                            // 同月區間
                            endDate = new DateTime(currentYear, currentMonth, endDay, 23, 59, 59);
                        }
                    }
                }

                // 根據計算後的 startDate, endDate 您可進行後續處理
                // 如需轉字串，可使用 startDate.ToString("yyyy-MM-dd")、endDate.ToString("yyyy-MM-dd")
                result.start_time = startDate;
                result.end_time = endDate;

                var data = new VDateRange
                {
                    StartTime = result.start_time,
                    EndTime = result.end_time
                };

               
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetDateRange");
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    code = "403",
                    message = "抓取資料異常",
                    data = new VDateRange()
                });
            }
        }

    }
}

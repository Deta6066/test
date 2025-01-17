using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VIS_API.Controllers.SLS;
using VisLibrary.Models.API;
using VisLibrary.Models;
using VisLibrary.Service.Interface;
using VisLibrary.Service;
using VisLibrary.UnitWork;
using VisLibrary.Models.JsonModel;
using AutoMapper;

namespace VIS_API.Controllers.Test
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IOrder _orderService;
        private readonly ILogger<OrderAndAmountController> _logger;
        //private readonly ITransactionManager _transactionManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper; // 注入IMapper

        public TestController(IUnitOfWork unitOfWork, IMapper mapper, IOrder orderService, ILogger<OrderAndAmountController> logger, IServiceProvider serviceProvider)
        {
            _orderService = orderService;
            _logger = logger;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }
        [HttpPost("TestOrderList")]
        public async Task<List<Order>> GetList([FromBody] OrderParameter parameters)
        {
            try
            {
                await _unitOfWork.InitializeAsync(); // 開始交易

                var companyDataRepository = _unitOfWork.GetRepository<MCompanyDataSource>();
                string sql = $"UPDATE company_data_source SET dbType = @dbType WHERE company_fk = @pk";
                var obj = new MCompanyDataSource
                {
                    dbType = 90,
                    company_fk = 2
                };
                //var parameters2 = new Dictionary<string, object?>
                //{
                //    { "@dbType", 99 },
                //    { "@pk", 2 }
                //};

                // 執行更新操作
                var rowsAffected = await companyDataRepository.Update(obj, "company_data_source");
                var test = await companyDataRepository.GetAll("company_data_source");
                await _unitOfWork.CommitAsync(); // 提交交易
            }
            catch (TimeoutException ex)
            {
                await _unitOfWork.RollbackAsync(); // 回滾交易
                _logger.LogError(ex, "Error occurred while processing GetOrderList.");
                throw;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync(); // 回滾交易
                _logger.LogError(ex, "Error occurred while processing GetOrderList.");
                throw;
            }
            finally
            {
                await _unitOfWork.DisposeAsync(); // 確保資源被正確釋放
            }


            // 新的作用域，新的 UnitOfWork 實例

            try
            {
                _unitOfWork.UpdateConnectionInfo("Data Source=192.168.1.46,5872;Database=detaVisHor_VM;User Id=sa;Password=asus54886961;TrustServerCertificate=true;", 1);
                await _unitOfWork.InitializeAsync(); // 開始新交易

                var processWorkLogModel = _unitOfWork.GetRepository<ProcessWorkLogModel>();
                var result = await processWorkLogModel.GetAll("processWorkLog");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing GetOrderList.");
                return new List<Order>();
            }
            finally
            {
                await _unitOfWork.DisposeAsync();
            }


            return new List<Order>();
        }


        [HttpPost]
        [Route("ActionResultAndApiResponseTest")]
        public async Task<ActionResult<ApiResponse<List<VAssembleCenter>>>> Test([FromBody] CompanyParameter parameters)
        {
            try
            {
                var assembleRepo = _unitOfWork.GetRepository<MAssembleCenter>();
                var data = await assembleRepo.GetAll("assemblecenter");
                var filteredData = data.Where(x => x.Company_fk == parameters.companyId).ToList();

                if (filteredData.Count == 0)
                {
                    // 無資料返回 NotFound 狀態
                    return NotFound(new ApiResponse<List<VAssembleCenter>>(
                        data: new List<VAssembleCenter>(),
                        success: false,
                        statusCode: ResponseStatusCode.NotFound,
                        message: "No data found."
                    ));
                }

                var dtoList = _mapper.Map<List<VAssembleCenter>>(filteredData);

                // 成功返回 Ok 狀態 (200) 並附上 ApiResponse
                return Ok(new ApiResponse<List<VAssembleCenter>>(
                    data: dtoList,
                    success: true,
                    statusCode: ResponseStatusCode.Ok,
                    message: null
                ));
            }
            catch (Exception ex)
            {
                // 發生例外返回 InternalServerError 狀態
                return NotFound(new ApiResponse<List<VAssembleCenter>>(
                    data: new List<VAssembleCenter>(),
                    success: false,
                    statusCode: ResponseStatusCode.InternalServerError,
                    message: ex.Message
                ));
            }
        }
    }
}

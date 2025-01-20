using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VIS_API.Mappings;
using VIS_API.Models;
using VIS_API.Repositories.Interface;
using VIS_API.Service.Interface;
using VIS_API.UnitWork;

namespace VIS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssembleCenterController : ControllerBase
    {
        private readonly ILogger<AssembleCenterController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; // 注入IMapper
        private readonly IAssembleCenter _assembleCenter;
        private readonly IRAssembleCenter _assembleCenterRepository;

        public AssembleCenterController(IRAssembleCenter assembleCenterRepository,IUnitOfWork unitOfWork, IMapper mapper, IAssembleCenter assembleCenter, ILogger<AssembleCenterController> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _assembleCenter = assembleCenter;
            _assembleCenterRepository = assembleCenterRepository;
        }

        [HttpPost]
        [Route("GetAssembleCenterList")]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var companyFk = int.Parse(User.FindFirst("companyFk").Value);

                var assembleRepo = _unitOfWork._assembleCenter;
                var data = await assembleRepo.GetAll("assemblecenter");
                var filteredData = data.Where(x => x.Company_fk == companyFk).ToList();

                // 假設無資料，返回空陣列及 success = true
                if (filteredData.Count == 0)
                {
                    return StatusCode(StatusCodes.Status204NoContent, new
                    {
                        code = "204",
                        message = "請求成功但無資料",
                        data = new List<VAssembleCenter>()
                    });
                }

                var dtoList = _mapper.Map<List<VAssembleCenter>>(filteredData);
                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    code = "403",
                    message = "抓取資料異常",
                    data = new List<VAssembleCenter>()
                });
            }
        }

        [HttpPost]
        [Route("GetAssembleCenterListTest")]
        public async Task<ActionResult<ApiResponse<List<VAssembleCenter>>>> GetListTest([FromBody] CompanyParameter parameters)
        {
            try
            {
                await _unitOfWork.OpenAsyncConnection();
                var s = await _unitOfWork._assembleCenter.GetByPkSS("2");
                var assembleRepo = _unitOfWork.GetRepository<MAssembleCenter>();
                var obj = new MAssembleCenter { Company_fk = parameters.companyId };
                var rs= await assembleRepo.GetBySearch(obj, "assemblecenter");
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

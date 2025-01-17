using Microsoft.AspNetCore.Mvc;
using VisLibrary.Models;
using VisLibrary.Service;
using VisLibrary.Service.Interface;

namespace VIS_API.Controllers.SLS
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnreturnedTransportRackController : Controller
    {
        ISUnreturnedTransportRack _sUnreturnedTransportRack;
        public UnreturnedTransportRackController(ISUnreturnedTransportRack sUnreturnedTransportRack)
        {
            _sUnreturnedTransportRack = sUnreturnedTransportRack;
        }
        [HttpPost("GetMUnreturnedTransports")]
        public async Task<ApiResponse<VMUnreturnedTransportRack>> GetMUnreturnedTransports(MUnreturnedTransportRackParamater filter)
        {
            VMUnreturnedTransportRack vm = new VMUnreturnedTransportRack() {UnreturnedTransportRackList=new List<MUnreturnedTransportRack>()};
            try
            {
                var result = await _sUnreturnedTransportRack.GetMUnreturnedTransports(filter);
                if (result == null)
                {
                    return new ApiResponse<VMUnreturnedTransportRack>(vm, success: false, message: "null");
                }
                vm.UnreturnedTransportRackList = result;
                return new ApiResponse<VMUnreturnedTransportRack>(vm, success: true);
                //return vm;
            }
            catch (Exception ex)
            {
                return new ApiResponse<VMUnreturnedTransportRack>(vm, success: false, message: ex.Message);
            }
         
        }
    }
}

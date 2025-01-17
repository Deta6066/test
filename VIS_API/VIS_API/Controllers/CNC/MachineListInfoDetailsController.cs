using Microsoft.AspNetCore.Mvc;
using VisLibrary.Models;
using VisLibrary.Models.CNC;
using VisLibrary.Service.Interface;
using VisLibrary.UnitWork;

namespace VIS_API.Controllers.CNC
{
    /// <summary>
    /// 設備問題回報表控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MachineListInfoDetailsController : Controller
    {
        ISStatus_history_info _status_History_Info;

        public MachineListInfoDetailsController(ISStatus_history_info status_History_Info)
        {
            _status_History_Info = status_History_Info;
        }
        /// <summary>
        /// 取得問題回報表
        /// </summary>
        /// <param name="filter">問題回報篩選器</param>
        /// <returns></returns>
        [HttpPost("GetMachineListInfoDetails")]
        public async Task<ApiResponse<VMStatusHistory_info>> GetMachineListInfoDetails(MStatus_history_infoParamater filter)
        {
            var response = new ApiResponse<VMStatusHistory_info>(new VMStatusHistory_info());
            VMStatusHistory_info vMStatusHistory_Info = new VMStatusHistory_info();
            List<MStatus_history_info> status_history_infoList = new List<MStatus_history_info>();
            List<MOperationalRate> status_realtime_infoList = new List<MOperationalRate>();
            try
            {
                status_history_infoList = await _status_History_Info.GetStatus_history_infoList(filter);
                //取得設備單日工時資料
                status_realtime_infoList = await _status_History_Info.GetStatus_RealTime_InfoList(filter);
                if (status_history_infoList != null)
                {
                    vMStatusHistory_Info.mStatus_History_InfoList = status_history_infoList;
                    response.Data = vMStatusHistory_Info;
                    response.Success = true;
                    response.Message = "success";
                }
                else
                {
                   response= new ApiResponse<VMStatusHistory_info>(vMStatusHistory_Info, false, message: "歷程紀錄為null");

                }

                if (status_realtime_infoList != null)
                {
                    vMStatusHistory_Info.mStatus_RealTime_InfoList = status_realtime_infoList;
                    response.Data = vMStatusHistory_Info;
                    response.Success = true;
                    response.Message = "success";
                }
                else
                {
                    if (response.Success == true)
                    {
                        response.Data = vMStatusHistory_Info;
                        response.Success = false;
                        response.Message = "實時資料為null";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message += " 歷程紀錄為null";

                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new ApiResponse<VMStatusHistory_info>(vMStatusHistory_Info, false, message: ex.Message);

            }

        }
    }
}

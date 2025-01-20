using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VIS_API.Models;
using VIS_API.Models.CNC;
using VIS_API.Models.View;
using VIS_API.Service.Interface;
using static VIS_API.Utilities.AllEnum;

namespace VIS_API.Controllers.CNC
{
    /// <summary>
    /// 生產監控面板控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MachineMonitoringDashboardController : Controller
    {
        ISMachineInfo _sMachineInfo;
        ISAps_info _sAps_info;
        ISStatus_currently_info _sStatus_Currently_Info;
        ISMachineGroup _sMachineGroup;
        ISMachineImage sMachineImage;
        IMapper _mapper;

        public MachineMonitoringDashboardController(ISMachineInfo sMachineInfo, ISAps_info sAps_info, ISStatus_currently_info sStatus_Currently_Info, ISMachineGroup sMachineGroup, ISMachineImage sMachineImage, IMapper mapper)
        {
            _sMachineInfo = sMachineInfo;
            _sAps_info = sAps_info;
            _sStatus_Currently_Info = sStatus_Currently_Info;
            _sMachineGroup = sMachineGroup;
            this.sMachineImage = sMachineImage;
            _mapper = mapper;
        }
        /// <summary>
        /// 取得設備資訊
        /// </summary>
        /// <returns></returns> 
        [HttpPost("GetMachineInfo")]
        public async Task<ApiResponse<VMachine_info>> GetMachineInfo([FromBody] MMachineInfoParameter parameter)
        {
            VMachine_info vMachine_Info = new VMachine_info();
            List<MMachineInfo> list = new List<MMachineInfo>();
            //透過Service取得資料
            MCompanyDataSource companyDataSource = new MCompanyDataSource();
            companyDataSource.dbType = (int)VISDbType.mysql;
            // parameter = new MMachineInfoParameter();
            try
            {
                List<MMachineImage> mMachineImages = new List<MMachineImage>();
                //取得設備圖片
                MMachineImageParameter mMachineImageParameter = new MMachineImageParameter();
                mMachineImageParameter.Company_fk = parameter.CompanyID;
                mMachineImageParameter.Area_fk = 1;
                mMachineImages = sMachineImage.GetMachineImages(mMachineImageParameter);
                list = await _sMachineInfo.GetMachineInfo(companyDataSource, parameter);
                if (list == null)
                {
                    return new ApiResponse<VMachine_info>(new VMachine_info(), success: false, message: "null");
                }
                vMachine_Info.MachineInfo = list;
                //透過AutoMapper將MMachineImage轉換成VMMachineImage
                vMachine_Info.MMachineImageList = _mapper.Map<List<VMMachineImage>>(mMachineImages);
                SetImageDataType(mMachineImages, vMachine_Info.MMachineImageList);
                vMachine_Info.ApsInfo = _sAps_info.GetAps_infoList(parameter.CompanyID);
                vMachine_Info.StatusCurrentlyInfo = _sStatus_Currently_Info.GetStatus_currently_infoList(parameter.AreaName);
                MMachineGroupParamater mMachineGroupParamater = new MMachineGroupParamater();
                mMachineGroupParamater.CompanyID = parameter.CompanyID;
                mMachineGroupParamater.AreaName = "德大本廠";
                vMachine_Info.MMachineGroupList = await _sMachineGroup.GetMachineGroupList(mMachineGroupParamater);
                return new ApiResponse<VMachine_info>(vMachine_Info, success: true);
                //   return vMachine_Info;
            }
            catch (Exception ex)
            {
                return new ApiResponse<VMachine_info>(new VMachine_info(), success: false, message: ex.Message);
            }

        }
        /// <summary>
        /// 設定圖片資料型態
        /// </summary>
        /// <param name="mMachineImages"></param>
        /// <param name="VMMachineImageList"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void SetImageDataType(List<MMachineImage> mMachineImages, List<VMMachineImage> VMMachineImageList)
        {
            for (int i = 0; i < mMachineImages.Count; i++)
            {
                if(mMachineImages[i].image_data!=null && mMachineImages[i].image_data.Length>4)
                {
                //取得image_data的開頭4byte 來判斷檔案格式
                byte[] image_data = mMachineImages[i].image_data;
                var hex = BitConverter.ToString(image_data, 0, 4).Replace("-", string.Empty);
                string dataHeader = "";
                switch (hex)
                {
                    case "FFD8FFE0":
                    case "FFD8FFE1":
                        dataHeader = "data:image/jpeg;base64,";
                        break;
                    case "89504E47":
                        dataHeader = "data:image/png;base64,";
                        break;
                    case "47494638":
                        dataHeader = "data:image/gif;base64,";
                        break;
                    case "424D":
                        dataHeader = "data:image/bmp;base64,";
                        break;
                    default:
                        dataHeader = "data:";
                        break;
                }
                VMMachineImageList[i].image_data_base64 = dataHeader + VMMachineImageList[i].image_data_base64;
                }
            }

        }

        /// <summary>
        /// 上傳設備圖片
        /// </summary>
        /// <param name="Company_fk">公司ID</param>
        /// <param name="Area_fk">廠區ID</param>
        /// <param name="MachineID">設備ID</param>
        /// <param name="file">設備圖片檔案</param>
        /// <returns></returns>
        [HttpPost("UploadMachineImage")]
        public async Task<ApiResponse<string>> UploadMachineImage(
            [FromForm] int Company_fk,
            [FromForm] int Area_fk,
            [FromForm] string MachineID,
            IFormFile file
            )
        {
            try
            {
                MUploadImageParamater paramater = new MUploadImageParamater();
                paramater.Company_fk = Company_fk.ToString();
                paramater.Area_fk = Area_fk;
                paramater.MachineID = MachineID;
                paramater.ImageFile = file;
                string error = "";
                sMachineImage.UploadMachineImage(paramater, out error);
                if (!string.IsNullOrEmpty(error))
                {
                    return new ApiResponse<string>("", success: false, message: "Upload Faild:" + error);
                }

                return new ApiResponse<string>("", success: true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>("", success: false, message: ex.Message);
            }
        }
    }
}

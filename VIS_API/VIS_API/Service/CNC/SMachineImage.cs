using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Models.CNC;
using VIS_API.Service.Interface;
using VIS_API.UnitWork;

namespace VIS_API.Service.CNC
{
    /// <summary>
    /// 設備圖片Service
    /// </summary>
    public class SMachineImage : ISMachineImage
    {
        IUnitOfWork _unitOfWork;
        public SMachineImage(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 上傳設備圖片
        /// </summary>
        /// <param name="paramater"></param>
        /// <returns></returns>
        public bool UploadMachineImage(MUploadImageParamater paramater, out string error)
        {
            //依公司ID與廠區ID 上傳圖片檔案到DB
            try
            {
                error = "";
                //取得repository
                string table_name = "machine_image";
                var repository = _unitOfWork.GetRepository<MMachineImage>();
                MMachineImage machineImage = new MMachineImage();
                using (var stream = new MemoryStream())
                {
                    paramater.ImageFile.CopyToAsync(stream);
                    machineImage.image_data = stream.ToArray();
                }
                machineImage.company_fk = paramater.Company_fk;
                machineImage.area_fk = paramater.Area_fk.ToString();
                machineImage.mach_id = paramater.MachineID;
                List<MMachineImage> currentImages = GetMachineImages(new MMachineImageParameter() { Company_fk =Convert.ToInt32(paramater.Company_fk),  Area_fk = paramater.Area_fk });
                //if db有資料就更新，沒有就新增
                bool hasData = currentImages.Any(x => x.mach_id == paramater.MachineID);
                int result= 0;
                if (hasData)
                {
                    machineImage.id = currentImages.FirstOrDefault(x => x.mach_id == paramater.MachineID).id;
                     result = repository.Update(machineImage, table_name).Result;
                    if (result == 0)
                    {
                        error = "上傳失敗";
                        return false;
                    }
                    return true;
                }
                 result = repository.Insert(machineImage, table_name).Result;
                if (result == 0)
                {
                    error = "上傳失敗";
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }

        }
        public List<MMachineImage> GetMachineImages(MMachineImageParameter filter) 
        {
            //依公司ID與廠區ID 取得圖片檔案
            try
            {
                string table_name = "machine_image";
                string sql = "SELECT * FROM machine_image WHERE company_fk = @company_fk AND area_fk = @area_fk";
                var repository = _unitOfWork.GetRepository<MMachineImage>();
                DynamicParameters parameters= new DynamicParameters(filter);
                //parameters.Add("@company_fk", filter.Company_fk);
                //parameters.Add("@area_fk", filter.Area_fk);
                var result = repository.GetBySql(sql, parameters).Result;
                return result;
            }
            catch (Exception ex)
            { 
                return null;
            }
        }

    }
}

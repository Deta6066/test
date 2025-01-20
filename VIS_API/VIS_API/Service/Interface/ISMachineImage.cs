using VIS_API.Models.CNC;

namespace VIS_API.Service.Interface
{
    public interface ISMachineImage
    {
        /// <summary>
        /// 取得設備圖片
        /// </summary>
        /// <param name="filter">取得設備圖片篩選器</param>
        /// <returns></returns>
        List<MMachineImage> GetMachineImages(MMachineImageParameter filter);

        /// <summary>
        /// 上傳設備圖片
        /// </summary>
        /// <param name="paramater"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        bool UploadMachineImage(MUploadImageParamater paramater, out string error);
    }
}
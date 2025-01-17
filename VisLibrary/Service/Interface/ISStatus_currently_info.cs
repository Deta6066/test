using VisLibrary.Models.CNC;

namespace VisLibrary.Service.Interface
{
    /// <summary>
    /// 設備當前狀態資訊服務
    /// </summary>
    public interface ISStatus_currently_info
    {
        /// <summary>
        /// 取得設備當前狀態資訊
        /// </summary>
        /// <param name="mach_name">設備名稱</param>
        /// <returns></returns>
        List<MStatus_currently_info> GetStatus_currently_infoList(string mach_name = "");
    }
}
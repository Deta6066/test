using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models.View
{
    /// <summary>
    /// 設備監控看板ViewModel
    /// </summary>
    public class VMMachineMonitoringDashboard
    {
        /// <summary>
        /// 設備名稱
        /// </summary>
        public string MachName { get; set; } = "";

        /// <summary>
        /// 設備狀態
        /// </summary>
        public string Status { get; set; } = "";

        /// <summary>
        /// 設備稼動率
        /// </summary>
        public string OperateRate { get; set; } = "";

        /// <summary>
        /// 主程式
        /// </summary>
        public string ProgMain { get; set; } = "";

        /// <summary>
        /// 主程式註解
        /// </summary>
        public string ProgMainCmd { get; set; } = "";

        /// <summary>
        /// 運行程式
        /// </summary>
        public string ProgRun { get; set; } = "";

        /// <summary>
        /// 運行程式註解
        /// </summary>
        public string ProgRunCmd { get; set; } = "";

        /// <summary>
        /// 生產進度(目前/預計)
        /// </summary>
        public string ProductRateDay { get; set; } = "";

        /// <summary>
        /// 運轉時間
        /// </summary>
        public string RunTime { get; set; } = "";

        /// <summary>
        /// 主軸轉速
        /// </summary>
        public string Acts { get; set; } = "";

        /// <summary>
        /// 校機人員
        /// </summary>
        public string CheckStaff { get; set; } = "";

        /// <summary>
        /// 操作人員
        /// </summary>
        public string WorkStaff { get; set; } = "";

        /// <summary>
        /// 工藝名稱
        /// </summary>
        public string CraftName { get; set; } = "";

        /// <summary>
        /// 應完工時間
        /// </summary>
        public string CompleteTime { get; set; } = "";

        /// <summary>
        /// 預計完工時間
        /// </summary>
        public string FinishTime { get; set; } = "";

        /// <summary>
        /// 主軸溫升
        /// </summary>
        public string SpindleTemp { get; set; } = "";

        /// <summary>
        /// 切削液溫度
        /// </summary>
        public string CoolantTemp { get; set; } = "";

        /// <summary>
        /// 伺服軸負載(X)
        /// </summary>
        public string ServoLoadingX { get; set; } = "";

        /// <summary>
        /// 伺服軸負載(Y)
        /// </summary>
        public string ServoLoadingY { get; set; } = "";

        /// <summary>
        /// 伺服軸負載(Z)
        /// </summary>
        public string ServoLoadingZ { get; set; } = "";

        /// <summary>
        /// 伺服軸負載(B)
        /// </summary>
        public string ServoLoadingB { get; set; } = "";

        /// <summary>
        /// 主軸負載(Deep)
        /// </summary>
        public string SpindleLoading { get; set; } = "";

        /// <summary>
        /// 製令單號
        /// </summary>
        public string ManuId { get; set; } = "";

        /// <summary>
        /// 客戶名稱
        /// </summary>
        public string CustomName { get; set; } = "";

        /// <summary>
        /// 產品名稱
        /// </summary>
        public string ProductName { get; set; } = "";

        /// <summary>
        /// 料件編號
        /// </summary>
        public string ProductNumber { get; set; } = "";

        /// <summary>
        /// 主軸溫度
        /// </summary>
        public string Spindletemp { get; set; } = "";

        /// <summary>
        /// 空壓源壓力
        /// </summary>
        public string AirPressure { get; set; } = "";

        /// <summary>
        /// 主軸速度
        /// </summary>
        public string SpindleSpeed { get; set; } = "";

        /// <summary>
        /// 切削時間
        /// </summary>
        public string CutTime { get; set; } = "";
    }


}

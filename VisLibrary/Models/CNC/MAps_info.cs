using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models.CNC
{
    /// <summary>
    /// 可視化資訊
    /// </summary>
    public class MAps_info
    {
        /// <summary>
        /// 設備ID
        /// </summary>
        public int _id { get; set; }
        /// <summary>
        /// 設備名稱
        /// </summary>
        public string mach_name { get; set; }
        /// <summary>
        /// 主軸轉速
        /// </summary>
        public string acts { get; set; }
        /// <summary>
        /// 操作人員
        /// </summary>
        public string work_staff { get; set; }
        /// <summary>
        /// 運行程式
        /// </summary>
        public string prog_run { get; set; }
        /// <summary>
        /// 運行程式註解
        /// </summary>
        public string prog_run_cmd { get; set; }
        /// <summary>
        /// 運轉時間
        /// </summary>
        public string run_time { get; set; }
        /// <summary>
        /// 主軸溫升
        /// </summary>
        public string Spindle_temp { get; set; }
        /// <summary>
        /// 切削時間
        /// </summary>
        public string cut_time { get; set; }
        /// <summary>
        /// 切削液溫度
        /// </summary>
        public string Coolant_Temp { get; set; }
        /// <summary>
        /// 伺服軸負載(X)
        /// </summary>
        public string Servo_loading_X { get; set; }
        /// <summary>
        /// 伺服軸負載(Y)
        /// </summary>
        public string Servo_loading_Y { get; set; }
        /// <summary>
        /// 伺服軸負載(Z)
        /// </summary>
        public string Servo_loading_Z { get; set; }
        /// <summary>
        /// 伺服軸負載(B)
        /// </summary>
        public string Servo_loading_B { get; set; }
        /// <summary>
        /// 設備稼動率
        /// </summary>
        public string operate_rate { get; set; }
        /// <summary>
        /// 主程式
        /// </summary>
        public string prog_main { get; set; }
        /// <summary>
        /// 主程式註解
        /// </summary>
        public string prog_main_cmd { get; set; }
        /// <summary>
        /// 設備狀態
        /// </summary>
        public string mach_status { get; set; }
        /// <summary>
        /// 預計完工時間
        /// </summary>
        public string finish_time { get; set; }
        /// <summary>
        /// 應完工時間
        /// </summary>
        public string complete_time { get; set; }
        /// <summary>
        /// 進給率
        /// </summary>
        public string @override { get; set; }
        /// <summary>
        /// 通電時間
        /// </summary>
        public string poweron_time { get; set; }
        /// <summary>
        /// 油箱油溫
        /// </summary>
        public string Tank_Hydraulic_Temp { get; set; }
        /// <summary>
        /// 主軸速度
        /// </summary>
        public string spindlespeed { get; set; }
        /// <summary>
        /// 製令單號
        /// </summary>
        public string manu_id { get; set; }
        /// <summary>
        /// 料件編號
        /// </summary>
        public string product_number { get; set; }
        /// <summary>
        /// 工藝名稱
        /// </summary>
        public string craft_name { get; set; }
        /// <summary>
        /// 異警資訊
        /// </summary>
        public string alarm_info { get; set; }
        /// <summary>
        /// 主軸負載
        /// </summary>
        public string Spindle_loading { get; set; }
        /// <summary>
        /// 主軸溫度
        /// </summary>
      public string  spindletemp { get; set; }
        /// <summary>
        /// 客戶名稱
        /// </summary>
        public string custom_name { get; set; }
        /// <summary>
        /// 生產進度
        /// </summary>
        public string product_rate_day { get; set; }
        /// <summary>
        /// 校機人員
        /// </summary>
        public string check_staff { get; set; }

    }
}

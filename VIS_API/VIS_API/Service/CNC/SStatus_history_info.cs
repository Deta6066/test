using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Models.CNC;
using VIS_API.Service.Interface;
using VIS_API.UnitWork;
using static VIS_API.Utilities.AllEnum;

namespace VIS_API.Service.CNC
{

    /// <summary>
    /// 問題歷程Service
    /// </summary>
    public class SStatus_history_info : ISStatus_history_info
    {
        IUnitOfWork _unitOfWork;

        public SStatus_history_info(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 取得問題歷程List
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<MStatus_history_info>> GetStatus_history_infoList(MStatus_history_infoParamater filter)
        {
            string tableName = "status_history_info";
            string condition = GetCondion(filter);
            List<MStatus_history_info> status_history_infoList = new List<MStatus_history_info>();
            string connectionString = "Server=192.168.101.171;port=3306;Database=cnc_db;User Id=root;Password=dek54886961;Charset=utf8;Convert Zero Datetime=True;AllowUserVariables=True;";
            string sql = $"SELECT * FROM {tableName} {condition}";
            _unitOfWork.UpdateConnectionInfo(connectionString, (int)VISDbType.mysql);
            var repository = _unitOfWork.GetRepository<MStatus_history_info>();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id", filter.CompanyID);
            parameters.Add("@mach_name", filter.mach_name);
            parameters.Add("@startDate", filter.select_date.ToString("yyyyMMdd000000"));
            parameters.Add("@endDate", filter.select_date.ToString("yyyyMMdd235959"));
            status_history_infoList = await repository.GetBySql(sql, parameters);
            return status_history_infoList;
        }
        public async Task<List<MOperationalRate>> GetStatus_RealTime_InfoList(MStatus_history_infoParamater filter)
        {
            string tableName = "status_realtime_info";
            string condition = GetRealTime_InfoCondion(filter);
            List<MOperationalRate> status_realtime_infoList = new List<MOperationalRate>();
            string connectionString = "Server=192.168.101.171;port=3306;Database=cnc_db;User Id=root;Password=dek54886961;Charset=utf8;Convert Zero Datetime=True;AllowUserVariables=True;";
            string sql = $"SELECT * FROM {tableName} {condition}";
            _unitOfWork.UpdateConnectionInfo(connectionString, (int)VISDbType.mysql);
            var repository = _unitOfWork.GetRepository<MOperationalRate>();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id", filter.CompanyID);
            parameters.Add("@mach_name", filter.mach_name);
          parameters.Add("@work_date", filter.select_date.ToString("yyyyMMdd"));
            status_realtime_infoList = await repository.GetBySql(sql, parameters);
            return status_realtime_infoList;
        }

        private static string GetCondion(MStatus_history_infoParamater filter)
        {
            string condition = "WHERE mach_name=@mach_name AND CAST(update_time AS double) >=@startDate AND CAST(enddate_time AS double)<=@endDate";
            // 如果日期為今天 則取得最後一筆資料
            condition = filter.select_date.Date == DateTime.Now.Date ? "ORDER BY _id DESC LIMIT 1" : condition;
            return condition;

        }
        private static string GetRealTime_InfoCondion(MStatus_history_infoParamater filter)
        {
            string condition = "WHERE mach_name=@mach_name AND work_date=@work_date";
            // 如果日期為今天 則取得最後一筆資料
            condition = filter.select_date.Date == DateTime.Now.Date ? "ORDER BY _id DESC LIMIT 1" : condition;
            return condition;
        }
    }
}

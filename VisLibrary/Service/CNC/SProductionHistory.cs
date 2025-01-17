using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VisLibrary.Models;
using VisLibrary.Models.CNC;
using VisLibrary.Service.Interface;
using VisLibrary.UnitWork;
using static Mysqlx.Expect.Open.Types;
using static VisLibrary.Utilities.AllEnum;

namespace VisLibrary.Service.CNC
{
    /// <summary>
    /// 生產履歷統計Service
    /// </summary>
    public class SProductionHistory : ISProductionHistory
    {
        UnitWork.IUnitOfWork _unitOfWork;
        ISMachineInfo _sMachineInfo;
        IConfiguration Configuration { get;set; }
        ISCompanyDataSource _sCompanyDataSource { get; set; }
        IAssembleCenter _sAssembleCenter { get; set; }

        public SProductionHistory(IUnitOfWork unitOfWork, ISMachineInfo sMachineInfo, IConfiguration configuration, ISCompanyDataSource sCompanyDataSource, IAssembleCenter sAssembleCenter) 
        {
            _unitOfWork = unitOfWork;
            _sMachineInfo = sMachineInfo;
            Configuration = configuration;
            _sCompanyDataSource = sCompanyDataSource;
            _sAssembleCenter = sAssembleCenter;
        }

        public async Task<List<MProductionHistory>> GetProductionHistory(MProductionHistoryDetailParamater filter, List<MAssembleCenter> assemblecenter)
        {
            List<MProductionHistory> mProductionHistoryDetails = new List<MProductionHistory>();
            string product_history_table = "cnc_db.product_history_info";
            //CompanyID = @CompanyID AND AreaID = @AreaID AND
            string cond = "where STR_TO_DATE(update_time, '%Y%m%d%H%i%s.%f') >= @StartTime AND STR_TO_DATE(enddate_time, '%Y%m%d%H%i%s.%f') <= @EndTime";
            try
            {
                //取得公司資料來源
                // var companyDataSource= await _sCompanyDataSource.GetDataSource(filter.CompanyID);
                //取得廠區資訊
                CompanyParameter companyParameter = new CompanyParameter();
                companyParameter.companyId = filter.CompanyID;
                await _unitOfWork.OpenAsyncConnection();
                string conn = "Server=192.168.101.171;port=3306;Database=cnc_db;User Id=root;Password=dek54886961;Charset=utf8;Convert Zero Datetime=True;AllowUserVariables=True;";
                _unitOfWork.UpdateConnectionInfo(conn, (int)VISDbType.mysql);
                string sql = @$"SELECT machine_info.mach_show_name AS MachineID, IFNULL(CONCAT(product_name, '-', craft_name), SUBSTRING_INDEX(program_history_info.main_prog, '/', -1)) AS 'RunProgram', sum(IFNULL(amount, 1)) as Amount, COUNT(*) AS Quantity FROM program_history_info, craft_info, machine_info WHERE update_time >= @StartTime AND enddate_time <= @EndTime  AND program_history_info.main_progflg = 'true' AND mach_show_name IS NOT NULL AND machine_info.mach_name = program_history_info.mach_name AND program_history_info.mach_name = craft_info.mach_name AND SUBSTRING_INDEX(program_history_info.main_prog, '/', -1) = craft_info.program GROUP BY MachineID , IFNULL(CONCAT(product_name, '-', craft_name), SUBSTRING_INDEX(program_history_info.main_prog, '/', -1))";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CompanyID", filter.CompanyID);
                parameters.Add("@AreaID", filter.AreaID);
                parameters.Add("@StartTime", filter.StartTime.ToString("yyyyMMdd000000"));
                parameters.Add("@EndTime", filter.EndTime.ToString("yyyyMMdd235959"));
                mProductionHistoryDetails = await _unitOfWork.GetRepository<MProductionHistory>().GetBySql(sql, parameters,100);
                // 取得篩選器群組ID對應的群組List
                var machineinfoParam = new MMachineInfoParameter();
                //machineinfoParam.areaName=filter.AreaID;
                machineinfoParam.AreaName=assemblecenter.Find(x => x.id == filter.AreaID).sName;
              //  var machineInfo = _sMachineInfo.GetMachineInfo(companyDataSource, machineinfoParam);
                //篩選 機台群組ID(機台群組待新增)

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return mProductionHistoryDetails;
        }
        /// <summary>
        /// 取得生產履歷統計明細
        /// </summary>
        /// <returns></returns>
        public async Task<List<MProductionHistoryDetail>> GetProductionHistoryDetail(MProductionHistoryDetailParamater filter)
        {
            List<MProductionHistoryDetail> mProductionHistoryDetails = new List<MProductionHistoryDetail>();
            
            try
            {
               // await  _unitOfWork.OpenAsyncConnection();
                string conn = "Server=192.168.101.171;port=3306;Database=cnc_db;User Id=root;Password=dek54886961;Charset=utf8;Convert Zero Datetime=True;AllowUserVariables=True;";
                _unitOfWork.UpdateConnectionInfo(conn, (int)VISDbType.mysql);
                string sql = GetSqlString(filter);
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CompanyID", filter.CompanyID);
                //parameters.Add("@AreaID", filter.AreaID);
                parameters.Add("@StartTime", filter.StartTime.ToString("yyyyMMdd000000"));
                parameters.Add("@EndTime", filter.EndTime.ToString("yyyyMMdd235959"));
                mProductionHistoryDetails = await _unitOfWork.GetRepository<MProductionHistoryDetail>().GetBySql(sql, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return mProductionHistoryDetails;
        }
        private string GetSqlString(MProductionHistoryDetailParamater filter)
        {
            string product_history_table = "program_history_info,craft_info,machine_info";
            string cond = "where update_time >= @StartTime AND enddate_time <= @EndTime AND program_history_info.main_progflg = 'true' AND mach_show_name IS NOT NULL AND machine_info.mach_name = program_history_info.mach_name AND program_history_info.mach_name = craft_info.mach_name AND SUBSTRING_INDEX(program_history_info.main_prog, '/', -1) = craft_info.program";
            string sql = @$"SELECT machine_info.mach_show_name MachineID,machine_info.mach_name as MachineName,IFNULL(CONCAT(product_name, '-', craft_name), SUBSTRING_INDEX(program_history_info.main_prog, '/', -1)) RunProgram, STR_TO_DATE(program_history_info.update_time, '%Y%m%d%H%i%s.%f')  StartTime, STR_TO_DATE(program_history_info.enddate_time, '%Y%m%d%H%i%s.%f')  EndTime,program_history_info.timespan ProcessTime FROM {product_history_table} {cond}";
            return sql;
        }
    }
}

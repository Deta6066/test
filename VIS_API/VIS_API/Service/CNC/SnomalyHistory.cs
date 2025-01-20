using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
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
    /// 異常歷程統計Service
    /// </summary>
    [Authorize]
    public class SNomalyHistory : ISnomalyHistory
    {
        IUnitOfWork _unitWork;
        IConfiguration _configuration;
        public SNomalyHistory(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitWork = unitOfWork;
            _configuration = configuration;
        }
        /// <summary>
        /// 取得異常歷程List
        /// </summary>
        public async Task<List<MAnomalyHistoryDetail>> GetAnomalyHistory(MAnomalyHistoryParameter filter, int companyID)
        {
            string table_name = "alarm_history_info";
           // string codition = " where Company_fk = @CompanyID And mach_name=@MachName ";
           string codition = " where mach_name =@mach_name";
            string sql = "SELECT * FROM " + table_name;
            //+ codition;
            string ConnectionString = "Server=192.168.101.171;port=3306;Database=cnc_db;User Id=root;Password=dek54886961;Charset=utf8;Convert Zero Datetime=True;AllowUserVariables=True;";
            _unitWork.UpdateConnectionInfo(ConnectionString,(int)VISDbType.mysql);
            Dapper.DynamicParameters parameters = new Dapper.DynamicParameters();
            parameters.Add("@mach_name", filter.MachName);
            List<MAnomalyHistoryDetail> anomalyHistoryList = new List<MAnomalyHistoryDetail>();
            var repository = _unitWork.GetRepository<MAnomalyHistoryDetail>();
            anomalyHistoryList= await repository.GetBySql(sql, parameters);
            return anomalyHistoryList;
        }
    }
}

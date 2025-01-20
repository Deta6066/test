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
    /// 設備群組Service
    /// </summary>
    public class SMachineGroup : ISMachineGroup
    {
        IUnitOfWork _unitOfWork;
        public SMachineGroup(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<MMachineGroup>> GetMachineGroupList(MMachineGroupParamater filter)
        {
            string tebleName = "mach_group";
            string connStr = "Server=192.168.101.171;port=3306;Database=cnc_db;User Id=root;Password=dek54886961;Charset=utf8;Convert Zero Datetime=True;AllowUserVariables=True;";
            string sql = $"SELECT * FROM {tebleName} where area_name=@AreaName";
            List<MMachineGroup> result = new List<MMachineGroup>();
            _unitOfWork.UpdateConnectionInfo(connStr, (int)VISDbType.mysql);
            var repository = _unitOfWork.GetRepository<MMachineGroup>();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompanyID", filter.CompanyID);
            parameters.Add("@AreaName", filter.AreaName);
            result = await repository.GetBySql(sql, parameters);
            return result;
        }
    }
}

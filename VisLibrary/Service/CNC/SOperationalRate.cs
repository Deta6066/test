using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models.CNC;
using VisLibrary.Service.Interface;
using VisLibrary.UnitWork;
using static VisLibrary.Utilities.AllEnum;

namespace VisLibrary.Service.CNC
{
    /// <summary>
    /// 稼動率分析Service
    /// </summary>
    public class SOperationalRate : ISOperationalRate
    {
        IUnitOfWork _unitOfWork;
        public SOperationalRate(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async  Task<List<MOperationalRate>> GetOperationalRate(MOperationalRateParamater param)
        {
            List<MOperationalRate> result = new List<MOperationalRate>();
            string condition = "";
            condition = $" where mach_name={param.mach_name}";
            string sql = "SELECT * FROM status_realtime_info;";
            DynamicParameters paramater= new DynamicParameters();
            string connectionString = "Server=192.168.101.171;port=3306;Database=cnc_db;User Id=root;Password=dek54886961;Charset=utf8;Convert Zero Datetime=True;AllowUserVariables=True;";
            _unitOfWork.UpdateConnectionInfo(connectionString,(int)VISDbType.mysql);
            var repository= _unitOfWork.GetRepository<MOperationalRate>();
            result = await repository.GetBySql(sql, paramater);
            return result;
        }
    }
}

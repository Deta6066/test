using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models;
using VisLibrary.Models.CNC;
using VisLibrary.Repositories.Interface;
using VisLibrary.Service.Interface;
using VisLibrary.UnitWork;
using VisLibrary.Utilities;
using static VisLibrary.Utilities.AllEnum;

namespace VisLibrary.Service
{
    public class SStatus_currently_info : ISStatus_currently_info
    {
        IServiceProvider _serviceProvider;
        private readonly ExceptionHandler _exceptionHandler;
        IUnitOfWork _unitOfWork;
        public SStatus_currently_info(IServiceProvider serviceProvider, ExceptionHandler exceptionHandler,IUnitOfWork unitOfWork)
        {
            _serviceProvider = serviceProvider;
            _exceptionHandler = exceptionHandler;
            _unitOfWork = unitOfWork;
        }
        public List<MStatus_currently_info> GetStatus_currently_infoList(string mach_name = "")
        {
            List<MStatus_currently_info> status_currently_infoList = new List<MStatus_currently_info>();
            string connectionString = "Server=192.168.101.171;port=3306;Database=cnc_db;User Id=root;Password=dek54886961;";
            _unitOfWork.UpdateConnectionInfo(connectionString, (int)VISDbType.mysql);
            var repository = _unitOfWork.GetRepository<MStatus_currently_info>();
           try
            {
                string cond = "";
                if (!string.IsNullOrWhiteSpace(mach_name))
                    cond = $"where mach_name={mach_name}";
                status_currently_infoList = repository.GetBySql($"SELECT * FROM status_currently_info {cond}", new()).Result;
            }
            catch (Exception ex)
            {
            }
            return status_currently_infoList;
        }
    }
}

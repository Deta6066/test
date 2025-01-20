using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Models;
using VIS_API.Models.WHE;
using VIS_API.Repositories.Interface;
using VIS_API.Service.Interface;
using VIS_API.UnitWork;
using VIS_API.Utilities;
using static VIS_API.Utilities.AllEnum;

namespace VIS_API.Service
{
    /// <summary>
    /// 產線服務
    /// </summary>
    public class SProductLine : ISProductLine
    {
        ExceptionHandler _exceptionHandler;
        IServiceProvider _serviceProvider;
        private readonly IUnitOfWork _unitOfWork;
        public SProductLine(ExceptionHandler exceptionHandler, IServiceProvider serviceProvider, IUnitOfWork unitOfWork)
        {
            _exceptionHandler = exceptionHandler;
            _serviceProvider = serviceProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MMachGroup>> GetProductLineGroupList(MaproductlinegpParamater filter)
        {
            try
            {
                string connstring = "Server=192.168.101.171;port=3306;Database=cnc_db;User Id=root;Password=dek54886961;Charset=utf8;Convert Zero Datetime=True;AllowUserVariables=True;";
                string tableName = "mach_group";
                _unitOfWork.UpdateConnectionInfo(connstring, (int)VISDbType.mysql);
                //  await _unitOfWork.OpenAsyncConnection(); // 開始交易
                var repository = _unitOfWork.GetRepository<MMachGroup>();

                //IRepository<Maproductlinegp> repository = _serviceProvider.GetRequiredService<IRepository<Maproductlinegp>>();
                List<MMachGroup> productLineGpList = new List<MMachGroup>();
                string sqlCond = "";
              //  sqlCond = GetGetProductLineGroupSlCond(filter);
                string sql = $"SELECT _id,group_name,mach_name as MachName,mach_show_name,area_name,add_account,web_address FROM {tableName} {sqlCond}";
                var parameters = new DynamicParameters();
                parameters.Add("@CompanyID", filter.CompanyID);
                parameters.Add("@AssemblecenterID", filter.AssemblecenterID);

                //_exceptionHandler.ExecuteWithExceptionHandling( () =>
                //{

                productLineGpList = repository.GetBySql(sql, parameters, 0).Result;
               // await _unitOfWork.CommitAsync();
                return productLineGpList;

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return new List<MMachGroup>();
                throw ex;

            }
            finally
            {
                await _unitOfWork.DisposeAsync();

            }

            // });

        }
        /// <summary>
        /// 取得產線群組條件
        /// </summary>
        /// <param name="filter">篩選器</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string GetGetProductLineGroupSlCond(MaproductlinegpParamater filter)
        {
            string cond = "";
            List<string> condList = new List<string>();
            if (!string.IsNullOrEmpty(filter.CompanyID))
            {
                condList.Add("Company_fk = @CompanyID");
            }
            if (filter.AssemblecenterID > 0)
            {
                condList.Add("Assemblecenter_fk = @AssemblecenterID");
            }
            if (condList.Count > 0)
            {
                cond = " WHERE " + string.Join(" AND ", condList);
            }
            else
            {
                cond = "";
            }
            return cond;
        }

        public List<Maproductline> GetProductLineList(MProductLineParameter filter)
        {
            //IRepository<Maproductline> repository= _serviceProvider.GetRequiredService<IRepository<Maproductline>>();
            string conn = "Server=192.168.101.171;port=3306;Database=dekvis;User Id=root;Password=dek54886961;Charset=utf8;Convert Zero Datetime=True;AllowUserVariables=True;";
            _unitOfWork.UpdateConnectionInfo(conn, (int)VISDbType.mysql);
            var repository = _unitOfWork.GetRepository<Maproductline>();
            List<Maproductline> productLineList = new List<Maproductline>();
            string sql = "SELECT * FROM aproductline WHERE Company_fk = @CompanyID";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompanyID", filter.CompanyID);
            _exceptionHandler.ExecuteWithExceptionHandling(() =>
            {
                productLineList = repository.GetBySql(sql, parameters, 0).Result;
            });

            return productLineList;
        }
        /// <summary>
        /// 取得產線群組名稱
        /// </summary>
        /// <param name="productLineGroupID">產線群組ID</param>
        /// <returns></returns>
        public string GetProductLineGroupName(int productLineGroupID)
        {
            IRepository<MMachGroup> repository = _serviceProvider.GetRequiredService<IRepository<MMachGroup>>();
            string sql = "SELECT * FROM aproductlinegp WHERE ID = @ID";
            ConcurrentDictionary<string, object?> keyValuePairs = new ConcurrentDictionary<string, object?>();
            keyValuePairs.TryAdd("@ID", productLineGroupID);
            MMachGroup productLineGroup = new MMachGroup();
            _exceptionHandler.ExecuteWithExceptionHandling(() =>
            {
                productLineGroup = repository.GetBySql(sql, keyValuePairs).Result.FirstOrDefault();
            });
            return productLineGroup.MachName ?? "";
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="FinishGoodDetail">成品庫存明細</param>
        /// <returns></returns>
        public string GetProductLineGroupName(MFinishGoodDetail FinishGoodDetail)
        {
            //取得產線群組ID
            throw new NotImplementedException();

        }
    }
}

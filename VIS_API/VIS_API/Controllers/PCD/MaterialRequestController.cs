using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using VIS_API.Controllers.SLS;
using VIS_API.Models;
using VIS_API.Models.JsonModel;
using VIS_API.Models.PCD;
using VIS_API.Models.View;
using VIS_API.Service.Interface;
using VIS_API.UnitWork;

namespace VIS_API.Controllers.PCD
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialRequestController : Controller
    {
        private readonly ILogger<OrderAndAmountController> _logger;
        //private readonly ITransactionManager _transactionManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        public MaterialRequestController(IUnitOfWork unitOfWork, ILogger<OrderAndAmountController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _serviceProvider = serviceProvider;
        }
        /// <summary>
        /// 取得領料單列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetMMaterialWithdrawalList")]
        public async Task<ApiResponse<VMMaterialWithdrawal>> GetMMaterialWithdrawalList(MMaterialWithdrawalParamater filter)
        {
            VMMaterialWithdrawal vMMaterialWithdrawal = new VMMaterialWithdrawal();
            List<MMaterialWithdrawal> materialWithdrawalList = new List<MMaterialWithdrawal>();
            List<MMaterialWithdrawalDetail> materialWithdrawalDetailList = new List<MMaterialWithdrawalDetail>();
            try
            {
                //去除空字串
                filter.BatchOrOrderNumber = filter.BatchOrOrderNumber.Where(x => !string.IsNullOrEmpty(x)).ToList();
                if (filter != null && filter.BatchOrOrderNumber.Count > 0)
                {
                    // 透過公司ID取得資料庫連線字串
                    //_unitOfWork.UpdateConnectionInfo("Server=172.23.9.101;port=3306;Database=new_dekviserp;User Id=dek;Password=54886961;Charset=utf8;Convert Zero Datetime=True;AllowUserVariables=True", 0); // 這裡要填入資料庫連線字串
                    await _unitOfWork.OpenAsyncConnection();
                    MCompanyDataSource companySource = await _unitOfWork.RCompanyDataSource.GetByPk(filter.CompanyID);
                    await _unitOfWork.CommitAsync(); // 結束交易
                    string connStr = companySource.dbParamater ?? "Server=192.168.1.210;Database=FJWSQL;User Id=dek;Password=asus54886961;TrustServerCertificate=True;";
                    _unitOfWork.UpdateConnectionInfo(connStr, companySource.dbType);
                    await _unitOfWork.InitializeAsync(); // 開始新交易
                    string cond = GetSqlCond(filter);
                    var MaterialWithdrawalModel = _unitOfWork.GetRepository<MMaterialWithdrawal>();
                    string sql = $"SELECT itemios.item_no AS ItemNumber, (SELECT itemnm FROM item WHERE item.item_no=itemios.item_no) AS ItemName, itemios.PLACE AS WarehouseLocation, itemios.unit AS Unit, cast(itemios.PQTY_H AS numeric(20, 2)) AS Inventory, cast(itemios.QTY1 AS numeric(20, 2)) AS RequestedQuantity, cast(itemios.QTY2 AS numeric(20, 2)) AS MaterialWithdrawalQuantity, cast(itemios.QTYNG AS numeric(20, 2)) AS ShortageQuantity, (SELECT SCLOSE FROM mkordis WHERE mkordis.trn_no=itemios.mkord_no AND mkordis.item_no=itemios.item_no) AS Status,ISnull(itemio.LOT_NO,itemio.mkord_no) BatchOrOrderNumber FROM itemios LEFT JOIN itemio ON itemios.trn_no = itemio.trn_no WHERE itemio.IO='1'{cond} order by itemios.item_no, cast(itemios.PQTY_H AS numeric(20, 2))";
                    // 取得領料單列表
                    materialWithdrawalList = await MaterialWithdrawalModel.GetBySql(sql, new DynamicParameters());
                    string detailSql = GetDetailSql(filter);
                    var MaterialWithdrawalDetailModel = _unitOfWork.GetRepository<MMaterialWithdrawalDetail>();
                    materialWithdrawalDetailList = await MaterialWithdrawalDetailModel.GetBySql(detailSql, new DynamicParameters());
                }
                else
                {
                    filter = new MMaterialWithdrawalParamater();
                }
                vMMaterialWithdrawal.MaterialWithdrawalList = materialWithdrawalList;
                vMMaterialWithdrawal.MMaterialWithdrawalDetails = materialWithdrawalDetailList;
                if(materialWithdrawalList == null )
                {
                    return new ApiResponse<VMMaterialWithdrawal>(new VMMaterialWithdrawal(), false, ResponseStatusCode.NoContent, "null");
                }
                return new ApiResponse<VMMaterialWithdrawal>(vMMaterialWithdrawal, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing GetOrderList.");

                return new ApiResponse<VMMaterialWithdrawal>
                    (new VMMaterialWithdrawal(),false,ResponseStatusCode.BadGateway, ex.Message);
            }
            finally
            {
                await _unitOfWork.DisposeAsync();
            }
            
        }

        private string GetDetailSql(MMaterialWithdrawalParamater filter)
        {
            string cond = "";
            // 取得篩選條件
            cond = GetDetailSqlCond(filter);
            // 取得領料單明細列表
            string detailSql = $"SELECT top(10000) itemio.trn_no AS MaterialWithdrawalNumber,itemio.trn_date AS MaterialWithdrawalDate, (SELECT name FROM employ WHERE itemio.user_code=employ.emp_no) AS MaterialReceiver,itemio.mkord_no AS ProductionOrderNumber, (SELECT cord_no FROM mkord WHERE mkord.trn_no=itemio.mkord_no) AS OrderNumber, itemio.s_desc AS UseExplain, (SELECT name FROM employ WHERE ITEMIO.STOCK=employ.emp_no) AS MaterialHandler,itemio.LOT_NO AS ManufacturingBatchNumber, (SELECT CUST.CUSTNM2 FROM CUST  WHERE CUST.cust_no= (SELECT cust_no FROM mkord WHERE mkord.trn_no=itemio.mkord_no)) AS CustomerName, itemio.MITEM_NO AS ProductComponents, cast(itemio.BOM_QTY AS numeric(20, 2)) AS MaterialWithdrawalQuantityOfMachine FROM itemio WHERE IO='1' {cond}";
            return detailSql;
        }

        /// <summary>
        /// 取得篩選條件
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string GetSqlCond(MMaterialWithdrawalParamater filter)
        {
            string cond = "";
            List<string> condList = new List<string>();
            if (filter.BatchOrOrderNumber.Count > 0)
            {
                string batchOrOrderNumber = string.Join("','", filter.BatchOrOrderNumber);
                // condList.Add($"isnull(itemio.LOT_NO,itemio.mkord_no) in ('{batchOrOrderNumber}')");
                condList.Add($"(itemio.LOT_NO in ('{batchOrOrderNumber}') OR itemio.mkord_no in('{batchOrOrderNumber}'))");

            }
            if (condList.Count > 0)
            {
                cond = string.Join(" AND ", condList);
            }
            cond = " and " + cond;
            return cond;
        }
        private string GetDetailSqlCond(MMaterialWithdrawalParamater filter)
        {
            string cond = "";
            List<string> condList = new List<string>();
            //檢查訂單號碼或製令單號碼是否有值
            if (filter.BatchOrOrderNumber.Count > 0)
            {
                string batchOrOrderNumber = string.Join("','", filter.BatchOrOrderNumber);
                //條件 : 製令單號(itemio.LOT_NO)或訂單號(itemio.mkord_no)
                condList.Add($"(itemio.LOT_NO in ('{batchOrOrderNumber}') OR itemio.mkord_no in('{batchOrOrderNumber}'))");
            }
            if (condList.Count > 0)
            {
                cond = string.Join(" AND ", condList);
            }
            cond = " and " + cond;
            return cond;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VisLibrary.Models;
using VisLibrary.Models.PMD;
using VisLibrary.Repositories.Interface;
using VisLibrary.Utilities;
using static VisLibrary.Utilities.AllEnum;

namespace VisLibrary.Service.PMD
{
    /// <summary>
    /// 產品推移圖服務
    /// </summary>
    public class SProductionShiftChart : ISProductionShiftChart
    {
        IServiceProvider _serviceProvider;
        IRCompanyDataSource _companyDataSource;
        public SProductionShiftChart(IServiceProvider serviceProvider, IRCompanyDataSource companyDataSource)
        {
            _serviceProvider = serviceProvider;
            _companyDataSource = companyDataSource;
        }

        ///<summary>
        /// 取得產品推移圖明細
        /// </summary>
        public List<Mwaitingfortheproduction_details> GetProductionShiftDetailList(WaitingfortheproductionParamater paramater)
        {
            //透過公司ID取得公司資料來源
            MCompanyDataSource companyDataSource = _companyDataSource.GetByPk(paramater.CompanyID, "company_data_source").Result;
            List<Mwaitingfortheproduction_details> result = new List<Mwaitingfortheproduction_details>();
            IRepository<Mwaitingfortheproduction_details> repository = _serviceProvider.GetService<IRepository<Mwaitingfortheproduction_details>>();
            StringBuilder sql =new StringBuilder();
            string startDate= "'20240101'";
            string endDate = "'20241231'";
            //從datasource取得sql語法
           string sqlCmd= GetSqlCmdFromDatasource(companyDataSource);
           // sql.AppendFormat("SELECT a.*,進度 Progress,狀態 Status,組裝日 AssemblyDate,a.ExpectedStartDate  ExpectedFinishDate , SUBSTRING(實際完成時間,1,8) FinishDate FROM (       SELECT       CUSTNM2 CustomerName,      FAB_USER ProductionLineNumber,      FAB_USER 工作站編號,      sw_MKORDSUB.LOT_NO ManufacturingBatchNumber,      A22_FAB.CORD_NO OrderNumber,      sw_CORD.CORD_NO  as CustomerOrderNumber,      sw_MKORDSUB.item_no as ItemNumber,      sw_item.itemnm as ItemName,       sw_cordsub.trn_date as OrderDate,      A22_FAB.STR_DATE as ExpectedStartDate ,      sw_MKORDSUB.SCLOSE ProductionOrderStatus       FROM SW.FJWSQL.dbo.A22_FAB       LEFT JOIN SW.FJWSQL.dbo.CORD AS sw_CORD ON sw_CORD.trn_no = A22_FAB.cord_no        LEFT JOIN SW.FJWSQL.dbo.CORDSUB AS sw_CORDSUB ON sw_CORDSUB.TRN_NO = A22_FAB.CORD_no AND sw_CORDSUB.SN = A22_FAB.CORD_SN       LEFT JOIN SW.FJWSQL.dbo.CUST AS sw_CUST ON sw_CUST.CUST_NO = sw_CORD.CUST_NO       LEFT JOIN SW.FJWSQL.dbo.MKORDSUB AS sw_MKORDSUB ON sw_MKORDSUB.CORD_NO = sw_CORDSUB.trn_no AND sw_MKORDSUB.CORD_SN = sw_CORDSUB.sn       LEFT JOIN SW.FJWSQL.dbo.citem AS sw_citem ON sw_CORDSUB.item_no = sw_citem.item_no       LEFT JOIN SW.FJWSQL.dbo.item_22 AS sw_item_22 ON sw_CORDSUB.item_no = sw_item_22.item_no       left join SW.FJWSQL.dbo.ITEM as sw_item on sw_item.ITEM_NO = sw_item_22.item_no       WHERE sw_MKORDSUB.FCLOSE <> 1 AND A22_FAB.STR_DATE > {0}AND       A22_FAB.STR_DATE <= {1} and 1<=FAB_USER and FAB_USER<=99 ) a left join 工作站狀態資料表 on 工作站狀態資料表.排程編號 = a.ManufacturingBatchNumber and 工作站狀態資料表.工作站編號 = a.ProductionLineNumber where (SUBSTRING(實際完成時間,1,8) >={0} OR 實際完成時間 is null OR 實際完成時間 = '') and ((a.ProductionOrderStatus = '結案' and 狀態 IS NOT NULL) OR (a.ProductionOrderStatus = '未結'))  order by a.ExpectedStartDate", startDate, endDate);
           sql.AppendFormat(sqlCmd, startDate, endDate);
            string mssql_ip = "192.168.1.46,5872";
            string dbname = "dekVisAssm_VM";
            string user = "sa";
            string password = "asus54886961";
            DbConnectionInfo connectionInfo = new DbConnectionInfo()
            { DbType = (int)VISDbType.mssql, ConnectionString = $"Server={mssql_ip};Database={dbname};Uid={user};Pwd={password};TrustServerCertificate=true;"
        };
            repository.UpdateConnectionInfo(connectionInfo);
            result = repository.GetBySql(sql.ToString(),new ConcurrentDictionary<string, object?>()).Result;
            // repository.UpdateConnectionInfo();
            return result;
        }

        private string GetSqlCmdFromDatasource(MCompanyDataSource? companyDataSource)
        {
            string result = string.Empty;
            SqlInfo sqlInfo=Utility.FromJson<SqlInfo>(companyDataSource.sqlcmd);
            if (sqlInfo != null)
            {
                result = sqlInfo.GetProductionShiftDetail;
            }
            return result;
        }
    }
}

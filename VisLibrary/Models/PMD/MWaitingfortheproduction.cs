using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Models.PMD
{
    /// <summary>
    /// 生產推移圖模型
    /// </summary>
    public class MWaitingfortheproduction
    {
        //Waitingfortheproduction=SELECT
        //a.*,進度,狀態,組裝日,a.預計開工日 預計完工日,
        //SUBSTRING(實際完成時間,1,8) 實際完成時間
        //FROM (       SELECT       CUSTNM2 客戶簡稱,      FAB_USER 產線代號,      FAB_USER 工作站編號,      sw_MKORDSUB.LOT_NO 排程編號,      A22_FAB.CORD_NO 訂單號碼,      sw_CORD.CORD_NO  as 客戶訂單,      sw_MKORDSUB.item_no as 品號,      sw_item.itemnm as 品名規格,       sw_cordsub.trn_date as 訂單日期,      A22_FAB.STR_DATE as 預計開工日,      sw_MKORDSUB.SCLOSE 製令狀態       FROM SW.FJWSQL.dbo.A22_FAB       LEFT JOIN SW.FJWSQL.dbo.CORD AS sw_CORD ON sw_CORD.trn_no = A22_FAB.cord_no        LEFT JOIN SW.FJWSQL.dbo.CORDSUB AS sw_CORDSUB ON sw_CORDSUB.TRN_NO = A22_FAB.CORD_no AND sw_CORDSUB.SN = A22_FAB.CORD_SN       LEFT JOIN SW.FJWSQL.dbo.CUST AS sw_CUST ON sw_CUST.CUST_NO = sw_CORD.CUST_NO       LEFT JOIN SW.FJWSQL.dbo.MKORDSUB AS sw_MKORDSUB ON sw_MKORDSUB.CORD_NO = sw_CORDSUB.trn_no AND sw_MKORDSUB.CORD_SN = sw_CORDSUB.sn       LEFT JOIN SW.FJWSQL.dbo.citem AS sw_citem ON sw_CORDSUB.item_no = sw_citem.item_no       LEFT JOIN SW.FJWSQL.dbo.item_22 AS sw_item_22 ON sw_CORDSUB.item_no = sw_item_22.item_no       left join SW.FJWSQL.dbo.ITEM as sw_item on sw_item.ITEM_NO = sw_item_22.item_no       WHERE sw_MKORDSUB.FCLOSE <> 1 AND A22_FAB.STR_DATE > {0}AND       A22_FAB.STR_DATE <= {2} and 1<=FAB_USER and FAB_USER<=99 ) a left join 工作站狀態資料表 on 工作站狀態資料表.排程編號 = a.排程編號 and 工作站狀態資料表.工作站編號 = a.產線代號 where (SUBSTRING(實際完成時間,1,8) >={0} OR 實際完成時間 is null OR 實際完成時間 = '') and ((a.製令狀態 = '結案' and 狀態 IS NOT NULL) OR (a.製令狀態 = '未結'))  order by a.預計開工日
        /// <summary>
        /// 客戶簡稱
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 產線代號
        /// </summary>
        public string ProductionLineNumber { get; set; }
        /// <summary>
        /// 排程編號/製造批號
        /// </summary>
        public string ManufacturingBatchNumber { get; set; }
        /// <summary>
        /// 訂單號碼
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// 客戶訂單號碼
        /// </summary>
        public string CustomerOrderNumber { get; set; }
        /// <summary>
        /// 品號
        /// </summary>
        public string ItemNumber { get; set; }
        /// <summary>
        /// 品名規格
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 訂單日期
        /// </summary>
        public string OrderDate { get; set; }
        /// <summary>
        /// 預計開工日
        /// </summary>
        public string ExpectedStartDate { get; set; }
        /// <summary>
        /// 預計完工日
        /// </summary>
        public string ExpectedFinishDate { get; set; }
        /// <summary>
        /// 實際完成時間
        /// </summary>
        public string FinishDate { get; set; }
        /// <summary>
        /// 製令狀態
        /// </summary>
        public string ProductionOrderStatus { get; set; }
        /// <summary>
        /// 進度
        /// </summary>
        public string Progress { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 組裝日
        /// </summary>
        public string AssemblyDate { get; set; }
    }
}

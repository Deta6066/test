using Dapper;
using NLog.Filters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIS_API.Models;
using VIS_API.Models.SLS;
using VIS_API.Service.Interface;
using VIS_API.UnitWork;
using static VIS_API.Utilities.AllEnum;

namespace VIS_API.Service
{
    /// <summary>
    /// 未歸還項目Service
    /// </summary>
    public class SUnreturnedTransportRack : ISUnreturnedTransportRack
    {
        private readonly IUnitOfWork _unitOfWork;
        public SUnreturnedTransportRack(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 取得未歸還項目List
        /// </summary>
        /// <returns></returns>
        public async Task<List<MUnreturnedTransportRack>> GetMUnreturnedTransports(MUnreturnedTransportRackParamater filter)
        {
            List<MUnreturnedTransportRack> mUnreturnedTransportRacks = new List<MUnreturnedTransportRack>();
            try
            {
                string sql = $"SELECT '('+a.客戶代號+')'+a.客戶簡稱 CustomerName,a.ItemName,ISNULL(sum(a.數量), 0) NormalQuantity ,ISNULL(sum(b.數量), 0) AbnormalQuantity FROM (SELECT cust.cust_no 客戶代號,cust.custnm2 AS 客戶簡稱,INVOSUB.ITEM_NO AS ItemName, (CASE WHEN cust.custnm2='MA' AND INVOSUB.ITEM_NO='MR4CM00ZZC07' THEN sum(INVOSUB.QUANTITY)-140 ELSE sum(INVOSUB.QUANTITY) END) AS 數量 FROM INVOSUB LEFT JOIN CUST AS cust ON cust.CUST_NO=INVOSUB.cust_no WHERE INVOSUB.CORD_NO=''   GROUP BY cust.custnm2, INVOSUB.ITEM_NO,cust.cust_no) a LEFT JOIN (SELECT cust.cust_no 客戶代號,cust.custnm2 AS 客戶簡稱,INVOSUB.ITEM_NO AS ItemName,sum(INVOSUB.QUANTITY) AS 數量 FROM INVOSUB LEFT JOIN CUST AS cust ON cust.CUST_NO=INVOSUB.cust_no WHERE INVOSUB.CORD_NO=''  GROUP BY cust.custnm2, INVOSUB.ITEM_NO,cust.cust_no HAVING SUM(INVOSUB.QUANTITY) < 0) b ON a.客戶簡稱 = b.客戶簡稱 AND a.ItemName = b.ItemName WHERE a.數量>0 GROUP BY a.客戶簡稱,a.ItemName,a.客戶代號";
                await _unitOfWork.OpenAsyncConnection();
                MCompanyDataSource _dataSource = await _unitOfWork.RCompanyDataSource.GetByPk(filter.CompanyID) ?? new MCompanyDataSource();
                int dbType = (int)VISDbType.mssql; 
                string connectionString = _dataSource.dbParamater;
                _unitOfWork.UpdateConnectionInfo(connectionString, dbType);
                DynamicParameters keyValuePairs = new DynamicParameters();
                mUnreturnedTransportRacks = _unitOfWork.GetRepository<MUnreturnedTransportRack>().GetBySql(sql, keyValuePairs).Result;
                //去除空白品項
                mUnreturnedTransportRacks = mUnreturnedTransportRacks.Where(x => !string.IsNullOrEmpty(x.ItemName)).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return mUnreturnedTransportRacks;
        }
    }
}

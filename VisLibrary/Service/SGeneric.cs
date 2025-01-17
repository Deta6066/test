using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models;
using VisLibrary.Models.API;
using VisLibrary.Repositories.Interface;
using VisLibrary.Service.Interface;

namespace VisLibrary.Service
{
    public class SGeneric : ISGeneric
    {
        private readonly IServiceProvider _serviceProvider;

        public SGeneric(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }

        //範例
        public async Task<List<Order>> GetList(OrderParameter? parameter = null)
        {
            var rCommon = _serviceProvider.GetService<IRepository<Order>>();


            //if filter is not null , set startDate and endDate
            if (parameter != null)
            {

                //取得公司資料來源
                //var result = await _companyDatasource.GetByPk(parameter.CompanyID);
                DbConnectionInfo newConnectionInfo;
                //if (result is not null)
                // {
                newConnectionInfo = new DbConnectionInfo { ConnectionString = "Server=192.168.1.26;Database=erp;Uid=jroot;Pwd=erp89886066;", DbType = 0 };
                rCommon.UpdateConnectionInfo(newConnectionInfo);
                //MSQL sQL = JsonConvert.DeserializeObject<MSQL>(result.sql);
                string sql = "SELECT DISTINCT item.pline_no AS productLineNo,\r\n                cust.custnm2 AS customerName,\r\n                mkordsub.item_no AS itemNumber,\r\n                mkordsub.lot_no AS lotNumber,\r\n                cordsub.sclose AS orderStatus,\r\n                employ.NAME AS salesman,\r\n                DATE_FORMAT(cordsub.d_date, '%Y%m%d') AS estimatedDeliveryDate,\r\n                DATE_FORMAT(mkordsub.str_date, '%Y/%m/%d') AS expectedStartDate,\r\n                itemios.trn_date AS receivedDate,\r\n                invosub.trn_date AS shipmentDate,\r\n                cordsub.quantity AS quantity,\r\n                cordsub.amount AS amount,\r\n                ( CASE\r\n                    WHEN SUBSTRING(DATE_FORMAT(cordsub.d_date, '%Y%m%d'), 7, 2) <= 25 THEN\r\n                      DATE_FORMAT(cordsub.d_date, '%Y%m')\r\n                    WHEN SUBSTRING(DATE_FORMAT(cordsub.d_date, '%Y%m%d'), 7, 2) >= 25 THEN\r\n                      CASE\r\n                        WHEN SUBSTRING(DATE_FORMAT(cordsub.d_date, '%Y%m%d'), 5, 2) = 12 THEN\r\n                          CONCAT(YEAR(cordsub.d_date) + 1, '01')\r\n                        ELSE\r\n                          DATE_FORMAT(DATE_ADD(cordsub.d_date, INTERVAL 1 MONTH), '%Y%m')\r\n                      END\r\n                  END ) AS orderMonth\r\nFROM cordsub\r\nLEFT JOIN item ON cordsub.item_no = item.item_no\r\nLEFT JOIN ws ON cordsub.trn_no = ws.cord_no AND cordsub.sn = ws.cord_sn\r\nLEFT JOIN mkordsub ON cordsub.trn_no = mkordsub.cord_no AND cordsub.sn = mkordsub.cord_sn\r\nLEFT JOIN cust ON cust.cust_no = cordsub.cust_no\r\nLEFT JOIN cord ON cord.trn_no = cordsub.trn_no\r\nLEFT JOIN invosub ON cordsub.trn_no = invosub.cord_no AND cordsub.sn = invosub.cord_sn\r\nLEFT JOIN employ ON employ.emp_no = cord.user_code\r\nLEFT JOIN (\r\n    SELECT itemios.cord_no,\r\n           itemios.cord_sn,\r\n           itemios.trn_date,\r\n           itemios.iotype\r\n    FROM erp.itemios\r\n    WHERE itemios.iotype = '3' AND itemios.s_desc <> '歸還'\r\n) AS itemios ON itemios.cord_no = cordsub.trn_no AND itemios.cord_sn = cordsub.sn\r\nWHERE item.pline_no > 0\r\n  AND ((CASE\r\n          WHEN invosub.trn_date IS NOT NULL THEN DATE(cordsub.d_date)\r\n          WHEN invosub.trn_date <= cordsub.d_date THEN DATE(invosub.trn_date)\r\n          ELSE DATE(cordsub.d_date)\r\n        END) BETWEEN @dateStrat AND @dateEnd\r\n       OR (cordsub.d_date BETWEEN @dateStrat AND @dateEnd))\r\n  AND cordsub.price > 0\r\nORDER BY cordsub.d_date ASC;\r\n";
                ConcurrentDictionary<string, object> keyValuePairs = new ConcurrentDictionary<string, object>();
                keyValuePairs.TryAdd("@dateStrat", parameter.StartDate);
                keyValuePairs.TryAdd("@dateEnd", parameter.EndDate);
               
                return await rCommon.GetBySql(sql, keyValuePairs); 
                
            }
            else
                return new List<Order>();
        }
    }
}

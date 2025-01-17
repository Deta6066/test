using Dapper;
using DapperDataBase.Database.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisLibrary.Models;
using VisLibrary.Models.PCD;
using VisLibrary.Models.WHE;
using VisLibrary.Repositories.Base;
using VisLibrary.Repositories.Interface;
using VisLibrary.Service.Base;
using VisLibrary.SqlGenerator;
using VisLibrary.Utilities;
using static VisLibrary.Utilities.AllEnum;

namespace VisLibrary.Repositories
{
    /// <summary>
    /// 成品庫存分析Repository
    /// </summary>
    public class RFinishedGoodsInventory : GenericRepository<MFinishGoodDetail>, IRFinishedGoodsInventory
    {
        public RFinishedGoodsInventory(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<MFinishGoodDetail> sqlGenerator)
     : base(propertyProcessor, db, sqlGenerator)
        {
        }
        public Task<int> Delete(int pk)
        {
            throw new NotImplementedException();
        }

        public Task<List<MFinishGoodDetail>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<MFinishGoodDetail>> GetAll(string sql, Dictionary<string, object?> prams)
        {
            throw new NotImplementedException();
        }

        public async Task<MFinishGoodDetail?> GetByPk(int? pk)
        {
            
            var parameters = new DynamicParameters();
            parameters.Add("@ID", pk);
            return (await _db.GetListAsync<MFinishGoodDetail>("ID=@ID", parameters)).FirstOrDefault();
        }

        public async Task<string> GetFinishedGoodsInventoryDetailSql(MInventoryParameter filter, int companyID, MCompanyDataSource? dataSource = null)
        {
            string cond = GetCondtion(filter);
            var _sqlCmd = Utility.FromJson<SqlInfo>(dataSource.sqlcmd);
            var _dbInfo = new DbConnectionInfo { ConnectionString = dataSource.dbParamater, DbType = dataSource.dbType };
            // string _sql = GetSqlcmdFromDataSource(cond, companyID);
            string _sql = _sqlCmd.GetFinsihedGoodsInventoryDetail ?? GetSqlcmdFromDataSource(cond, companyID);
            //內部資料庫查詢成品庫存明細
            Dictionary<string, object?> dic = new Dictionary<string, object?>();
            dic.Add("@company_fk", companyID);
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.Factory_fk))
                {
                    dic.Add("@factory_fk", filter.Factory_fk);
                }
            }
           // return await _db.GetListAsync<MFinishGoodDetail>(_sql, dic);
           return _sql;
        }
        /// <summary>
        /// 從資料來源取得sql語法
        /// </summary>
        /// <param name="cond">篩選條件</param>
        /// <param name="companyID">公司ID</param>
        /// <returns></returns>
        private string GetSqlcmdFromDataSource(string cond, int companyID)
        {
            // TODO: sql語法改成從company_data_source取得
            string sql = $"SELECT * FROM finishgooddetail WHERE company_fk = @company_fk {cond}";
            string _sqltest = "SELECT 0 as ID, a.@客戶簡稱,a.@產線代號,a.@製造批號,a.@入庫日,DATEDIFF(DAY, Cast(a.@入庫日 AS DATETIME), GETUTCDATE()) @庫存天數,a.@庫存金額,a.@庫存原因 FROM (SELECT cust.custnm2 AS @客戶簡稱,A22_FAB.FAB_USER AS @產線代號 ,mkordsub.LOT_NO @製造批號, (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS, ITEMIO WHERE ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO AND ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') @入庫日, cordsub.AMOUNT @庫存金額,A22_FAB.USER_FIELD08 @庫存原因 FROM A22_FAB, CORDSUB, MKORDSUB, CUST,item WHERE A22_FAB.CORD_NO=cordsub.TRN_NO AND A22_FAB.CORD_SN=cordsub.SN AND mkordSUB.CORD_NO=CORDSUB.trn_no AND mkordsub.CORD_SN=cordsub.sn AND CUST.CUST_NO=CORDsub.CUST_NO and CORDSUB.ITEM_NO=item.ITEM_NO AND cordsub.SCLOSE !='結案' AND item.class = 'Z' ) a WHERE a.@入庫日 <> '' and 1<=a.@產線代號 and a.@產線代號<=99";
            return _sqltest;
        }

        public Task<int> Insert(MFinishGoodDetail obj, bool autoIncrement = true)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(MFinishGoodDetail obj)
        {
            throw new NotImplementedException();
        }
        private string GetCondtion(MInventoryParameter filter)
        {
            string cond = "";
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.Factory_fk))
                {
                    // cond += $" AND factory_fk = '@factory_fk'";
                }
            }
            return cond;
        }
    }
}

using DapperDataBase.Database.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VIS_API.Models;
using VIS_API.Models.PCD;
using VIS_API.Repositories.Base;
using VIS_API.Repositories.Interface;
using VIS_API.Service.Base;
using VIS_API.SqlGenerator;
using VIS_API.Utilities;

namespace VIS_API.Repositories
{
    public class RHistoricalMaterialUsage :
        GenericRepositoryBase<HistoricalMaterialUsageModel>, IRHistoricalMaterialUsage
    {
        public RHistoricalMaterialUsage(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<HistoricalMaterialUsageModel> sqlGenerator)
        : base(propertyProcessor, db, sqlGenerator)
        {
        }
        const string TableName = "historicalmaterialusage_detail";
        public  Task<int> Delete(int pk)
        {
            throw new NotImplementedException();
        }

        public  Task<List<HistoricalMaterialUsageModel>> GetAll()
        {
            string sql = $"SELECT * FROM {TableName}";
            return _db.GetListAsync<HistoricalMaterialUsageModel>(sql, null);
        }

        public Task<List<HistoricalMaterialUsageModel>> GetAll(string sql, Dictionary<string, object?> prams)
        {
            throw new NotImplementedException();
        }

        public  Task<HistoricalMaterialUsageModel?> GetByPk(int? pk)
        {
            throw new NotImplementedException();
        }

        public List<HistoricalMaterialUsageModel> GetHistoricalMaterials(MHistoricalMaterialUsageParameter filter, int CompanyID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<HistoricalMaterialUsageModel_Detail>> GetHistoricalMaterialUsageModel_DetailAsync(MHistoricalMaterialUsageParameter filter)
        {
            // string sql = $@"SELECT * FROM {TableName} WHERE company_fk = {companyID}";
            //領料單號別名
            string TrnNoAlias = "MaterialRequisitionID";
            //品號別名
            string MaterialNoAlias = "MaterialRequisitionID";
            //領料單日期別名
            string TrnDateAlias = "MaterialDate";
            //領料單明細品號別名
            string MaterialDescriptionNoAlias = "MaterialDescriptionNo";
            //領料單明細品名規格別名
            string MaterialDescriptionAlias = "MaterialDescription";
            //領料數量別名
            string Qty2Alias = "MaterialQuantity";
            //用途說明別名
            string UsageDescriptionAlias = "UsageDescription";
            //製令單號別名
            string ProductionOrderNoAlias = "ProductionOrderID";
            //製令明細品號別名
            string ProductionOrderDetailNoAlias = "ProductionOrderDetailID";
            //製令明細品名規格別名
            string ProductionOrderDetailAlias = "ProductionOrderDetailDescription";
            //使用客戶別名
            string CustomerAlias = "CustomerName";


            string trn_date_start = "20240701";
            //交易起始日
            trn_date_start=filter.MaterialRequisitionDateStart.ToString("yyyyMMdd");
            //交易結束日
            string trn_date_end = "20240731";
            trn_date_end=filter.MaterialRequisitionDateEnd.ToString("yyyyMMdd");
            string sql = $@"SELECT top(10000)  ITEMIOS.TRN_NO AS {TrnNoAlias}, ITEMIOS.TRN_date AS {TrnDateAlias}, ITEMIOS.item_no AS {MaterialDescriptionNoAlias}, ITEM.itemnm AS {MaterialDescriptionAlias}, itemios.qty2 AS {Qty2Alias}, (CASE WHEN CHARINDEX('(', ITEMIOS.S_DESC COLLATE Latin1_General_CS_AS) > 0 THEN SUBSTRING(ITEMIOS.S_DESC, 0, CHARINDEX('(', ITEMIOS.S_DESC)) ELSE ITEMIOS.S_DESC END) AS {UsageDescriptionAlias}, itemios.mkord_no AS {ProductionOrderNoAlias}, (SELECT top 1 mkordsub.ITEM_NO FROM mkordsub WHERE ITEMIOS.MKORD_NO = mkordsub.TRN_NO) AS {ProductionOrderDetailNoAlias}, (SELECT item.ITEMNM FROM item WHERE item.ITEM_NO = (SELECT top 1 mkordsub.ITEM_NO FROM mkordsub WHERE ITEMIOS.MKORD_NO = mkordsub.TRN_NO)) AS {ProductionOrderDetailAlias}, ISNULL(CUST.CUSTNM2,'未填寫') AS {CustomerAlias} FROM ITEMIOS LEFT JOIN item AS item ON ITEMIOS.ITEM_NO = item.ITEM_NO LEFT JOIN CUST AS CUST ON itemios.CUST_NO = CUST.CUST_NO WHERE  TRN_DATE>= '{trn_date_start}' AND TRN_DATE<= {trn_date_end} AND itemios.qty2 >0  ORDER BY ITEMIOS.TRN_date";
            if(!string.IsNullOrEmpty(filter.MaterialNo))
            {
               // sql += $" AND materialDescriptionNo = '{MaterialNo}'";
            }
            //新增測試資料
            List<HistoricalMaterialUsageModel_Detail> list = new List<HistoricalMaterialUsageModel_Detail>();
            list = await _db.GetListAsync<HistoricalMaterialUsageModel_Detail>(sql, null,0);
            return list;
        }

        public Task<int> Insert(HistoricalMaterialUsageModel obj, bool autoIncrement = true)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(HistoricalMaterialUsageModel obj)
        {
            throw new NotImplementedException();
        }
    }
}

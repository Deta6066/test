using DapperDataBase.Database.Interface;
using Microsoft.AspNetCore.Http;

using VIS_API.Models;
using VIS_API.Repositories;
using VIS_API.Repositories.Base;
using VIS_API.Repositories.Interface;
using VIS_API.Service.Base;
using VIS_API.SqlGenerator;
using VIS_API.Utilities;

namespace Repositories
{
    public class RSupplierMaterial : GenericRepository<MSupplierMaterialShortAge>, IRSupplierMaterial
    {
        public RSupplierMaterial(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<MSupplierMaterialShortAge> sqlGenerator)
           : base(propertyProcessor, db, sqlGenerator)
        {
        }
        string UrgentMaterialNo_Alias = "UrgentMaterialNo";
        string TRN_No_Alias = "TransactionNumber";
        //開單日期
        string trn_date_Alias = "PurchaseOrderIssueDate";
        string fact_no_Alias = "FactoryNumber";
        string factnm2_Alias = "FactoryName";
        string item_no_Alias = "ItemNumber";
        string itemName_Alias = "ItemName";
        //催料預交日
        string d_date_Alias = "DeadLineDate";
        string MaterialRequisitionQuantity_Alias = "MaterialRequisitionQuantity";
        //未交量
        string UnpaidQuantity_Alias = "NotGiven";
        //加工代號
        string ProcessingCode_Alias = "MachiningNumber";

        public Task<int> Delete(int pk)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MSupplierMaterialShortAge>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<MSupplierMaterialShortAge>> GetAll(string sql, Dictionary<string, object?> prams)
        {
            throw new NotImplementedException();
        }

        public Task<MSupplierMaterialShortAge?> GetByPk(int? pk)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MSupplierMaterialShortAge>> GetSupplierMaterialShortAge(int companyID, MSupplierMaterialParameter filter)
        {
            //string cond = "URGE_ITEM.trn_no ='130715001'";
            //若有篩選器則加入篩選條件
            //依filter取得篩選條件字串
            string cond = GetSqlCondtion(filter);
            List<MSupplierMaterialShortAge> result = new List<MSupplierMaterialShortAge>();

            //透過公司ID取得供應商未交物料
            // string sql = $@"SELECT * FROM suppliershortage_urge WHERE company_fk = {companyID} {cond}";
            string sql = $"SELECT top(10000)  * from ( select  URGE_ITEM.trn_no AS {UrgentMaterialNo_Alias}, ORIGIN_TRN_NO AS '{TRN_No_Alias}', CONVERT(datetime, fjwsql.dbo.SORD.TRN_DATE, 112) AS '{trn_date_Alias}', URGE_ITEM.FACT_NO AS {fact_no_Alias}, factnm2 AS {factnm2_Alias}, ITEM_NO AS {item_no_Alias}, itemnm AS {itemName_Alias}, D_DATE AS '{d_date_Alias}', LEFT(sum(quantity), CHARINDEX('.', sum(quantity)) - 1) AS '{MaterialRequisitionQuantity_Alias}', 0 AS {UnpaidQuantity_Alias} ,URGE_ITEM.SOITEM_NO {ProcessingCode_Alias} FROM URGE_ITEM LEFT JOIN fjwsql.dbo.fact ON URGE_ITEM.fact_no = fact.fact_no LEFT JOIN fjwsql.dbo.SORD ON urge_item.ORIGIN_TRN_NO = sord.TRN_NO  GROUP BY ORIGIN_TRN_NO, URGE_ITEM.FACT_NO, ITEM_NO, itemnm, D_DATE, factnm2, fjwsql.dbo.SORD.TRN_DATE, URGE_ITEM.trn_no,URGE_ITEM.SOITEM_NO ) AS SubQuery  {cond}";
            //執行SQL取得供應商未交物料
            result = await _db.GetListAsync<MSupplierMaterialShortAge>(sql, null);
            return result;
        }

        public Task<int> Insert(MSupplierMaterialShortAge obj, bool autoIncrement = true)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(MSupplierMaterialShortAge obj)
        {
            throw new NotImplementedException();
        }

        private string GetSqlCondtion(MSupplierMaterialParameter filter)
        {
            string cond = "";
            // bool HasUrgentMaterialNo= !string.IsNullOrEmpty(filter.UrgentMaterialNo);
            List<string> condList = new List<string>();

            // condList.Add($"URGE_ITEM.trn_no = '{filter.UrgentMaterialNo}'");
            if (!string.IsNullOrEmpty(filter.UrgentMaterialNo))
                condList.Add($"{UrgentMaterialNo_Alias} = '{filter.UrgentMaterialNo}'");
            //if (!string.IsNullOrEmpty(filter.FactoryNumber))
            if (filter.FactoryNumber.Count > 0)
            {
                string factoryNumber = string.Join("','", filter.FactoryNumber);
                condList.Add($"{fact_no_Alias} in ('{factoryNumber}')");
            }
            //{
            //    condList.Add($"{fact_no_Alias} = '{filter.FactoryNumber}'");
            //}
            //if (!string.IsNullOrEmpty(filter.FactoryName))
            //{
            //      condList.Add($"{factnm2_Alias} = '{filter.FactoryName}'");
            //}
            if (filter.FactoryName.Count > 0)
            {
                string factoryName = string.Join("','", filter.FactoryName);
                condList.Add($"{factnm2_Alias} in ('{factoryName}')");
            }
            //判斷是否有品號
            if (!string.IsNullOrEmpty(filter.ItemNumber))
            {
                //若有則加入篩選條件
                condList.Add($"{item_no_Alias} = '{filter.ItemNumber}'");
            }

            //判斷是否有傳入催料預交日 開始
            if (filter.UrgentMaterialDeliveryDateStart != null)
            {
                //若有則加入篩選條件
                condList.Add($"{d_date_Alias} >= '{filter.UrgentMaterialDeliveryDateStart.Value.ToString("yyyy-MM-dd")}'");
            }
            //判斷是否有傳入催料預交日 結束
            if (filter.UrgentMaterialDeliveryDateEnd != null)
            {
                //若有則加入篩選條件
                condList.Add($"{d_date_Alias} <= '{filter.UrgentMaterialDeliveryDateEnd.Value.ToString("yyyy-MM-dd")}'");
            }


            //若有篩選條件則加入篩選條件
            if (condList.Count > 0)
            {
                //將篩選條件用AND串接， 第一個條件不加AND
                cond = string.Join(" AND ", condList);
            }
            //if cond is not empty, add WHERE
            if (!string.IsNullOrEmpty(cond))
            {
                cond = $"WHERE {cond}";
            }
            return cond;
        }
    }
}

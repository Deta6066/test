using Dapper;
using DapperDataBase.Database.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisLibrary.Models;
using VisLibrary.Models.WHE;
using VisLibrary.Repositories.Base;
using VisLibrary.Repositories.Interface;
using VisLibrary.Service.Base;
using VisLibrary.SqlGenerator;
using VisLibrary.Utilities;

namespace VisLibrary.Repositories
{
    public class RScrapQuantity  : GenericRepository<MScrapQuantity>, IRScrapQuantity
    {
        public RScrapQuantity(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<MScrapQuantity> sqlGenerator)
  : base(propertyProcessor, db, sqlGenerator)
        {
        }

        const string TableName = "scrap_quantity";
        
        public  Task<int> Delete(int pk)

        {
            string sql = $"DELETE FROM {TableName} WHERE ScrapQuantityID = @ID";
            var parameters = new DynamicParameters();
            parameters.Add("@ID", pk);
            return _db.ExecuteNonQueryAsync(sql, parameters);
        }

        public  async  Task<List<MScrapQuantity>> GetAll()
        {
            string sql = $"SELECT * FROM {TableName}";
            return await _db.GetListAsync<MScrapQuantity>(sql, null);
        }

        public  async Task<MScrapQuantity?> GetByPk(int? pk)
        {
            string sql = $"SELECT * FROM {TableName} where ScrapQuantityID =@ID";
            
            var parameters = new DynamicParameters();
            parameters.Add("@ID", pk);
            return (await _db.GetListAsync<MScrapQuantity>(sql, parameters)).FirstOrDefault(new MScrapQuantity());
        }

        public  async Task<int> Insert(MScrapQuantity obj, bool autoIncrement = true)
        {
            string cols = "";
            // string cols = Utility.CmdColStr<MScrapQuantity>(obj);
            cols = "ScrapPersonnel,MaterialWithdrawalNumber,MaterialWithdrawalDate,ItemNumber,ItemName,Unit,StockCost,ScraQuantity,TotalScrapCost,Remark,company_fk";
            string vals = "";
            //string vals = Utility.CmdCValueStr<MScrapQuantity>(obj);
            vals = "@ScrapPersonnel,@MaterialWithdrawalNumber,@MaterialWithdrawalDate,@ItemNumber,@ItemName,@Unit,@StockCost,@ScraQuantity,@TotalScrapCost,@Remark,@company_fk";
            string sql = $@"INSERT INTO {TableName} ({cols}) VALUES({vals})";
            var parameters = new DynamicParameters();
            parameters.Add("@ScrapPersonnel", obj.ScrapPersonnel);
            parameters.Add("@MaterialWithdrawalNumber", obj.MaterialWithdrawalNumber);
            parameters.Add("@MaterialWithdrawalDate", obj.MaterialWithdrawalDate);
            parameters.Add("@ItemNumber", obj.ItemNumber);
            parameters.Add("@ItemName", obj.ItemName);
            parameters.Add("@Unit", obj.Unit);
            parameters.Add("@StockCost", obj.StockCost);
            parameters.Add("@ScraQuantity", obj.ScraQuantity);
            parameters.Add("@TotalScrapCost", obj.TotalScrapCost);
            parameters.Add("@Remark", obj.Remark);
            parameters.Add("@company_fk", obj.company_fk);
            return await _db.ExecuteNonQueryAsync(sql, parameters);
        }

        public  Task<int> Update(MScrapQuantity obj)
        {
            var parameters = new DynamicParameters();
            string sql = $"UPDATE {TableName} SET `ScrapPersonnel` = @ScrapPersonnel, `MaterialWithdrawalNumber` = @MaterialWithdrawalNumber, `MaterialWithdrawalDate` = @MaterialWithdrawalDate, `ItemNumber` = @ItemNumber, `ItemName` = @ItemName, `Unit` = @Unit, `StockCost` = @StockCost, `ScraQuantity` = @ScraQuantity, `TotalScrapCost` = @TotalScrapCost, `Remark` = @Remark, `company_fk` = @company_fk WHERE `id` =@id; ";
            parameters.Add("@ScrapPersonnel", obj.ScrapPersonnel);
            parameters.Add("@MaterialWithdrawalNumber", obj.MaterialWithdrawalNumber);
            parameters.Add("@MaterialWithdrawalDate", obj.MaterialWithdrawalDate);
            parameters.Add("@ItemNumber", obj.ItemNumber);
            parameters.Add("@ItemName", obj.ItemName);
            parameters.Add("@Unit", obj.Unit);
            parameters .Add("@StockCost", obj.StockCost);
            parameters.Add("@ScraQuantity", obj.ScraQuantity);
            parameters.Add("@TotalScrapCost", obj.TotalScrapCost);
            parameters.Add("@Remark", obj.Remark);
            parameters.Add("@company_fk", obj.company_fk);
            parameters.Add("@id", obj.ID);
            //更新資料
            return _db.ExecuteNonQueryAsync(sql, parameters);
        }

       

        public List<MScrapQuantity> GetScrapQuantityList(int companyID, MScrapQuantityParameter filter)
        {
            List<MScrapQuantity> list = new List<MScrapQuantity>();
            string cond = GetSqlCondition(filter);
            string sql = $"SELECT * FROM {TableName} where company_fk = @companyID {cond}";
            
            var parameters = new DynamicParameters();
            parameters.Add("@companyID", companyID);
            
            if (filter != null)
            {
                if (filter.StartDate != DateTime.MinValue)
                {
                    parameters.Add("@startDate", filter.StartDate);
                }
                if (filter.EndDate != DateTime.MinValue)
                {
                    parameters.Add("@endDate", filter.EndDate);
                }
            }
            list = _db.GetListAsync<MScrapQuantity>(sql, parameters).Result;
            return list;
        }

        private string GetSqlCondition(MScrapQuantityParameter filter)
        {
            string cond = "";
            if (filter != null)
            {
                if (filter.StartDate != DateTime.MinValue)
                {
                    cond += $" and MaterialWithdrawalDate >= @startDate";
                }
                if (filter.EndDate != DateTime.MinValue)
                {
                    cond += $" and MaterialWithdrawalDate <= @endDate";
                }
            }
            return cond;
        }

    }
}

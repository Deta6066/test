using Dapper;
using DapperDataBase.Database.Interface;
using Mysqlx.Expr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisLibrary.Models;
using VisLibrary.Repositories.Base;
using VisLibrary.Repositories.Interface;
using VisLibrary.Service.Base;
using VisLibrary.SqlGenerator;
using VisLibrary.Utilities;

namespace VisLibrary.Repositories
{
    public class RSupplierScore : GenericRepository<MSupplierScoreDetail>,  IRSupplierScore
    {
        public RSupplierScore(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<MSupplierScoreDetail> sqlGenerator)
          : base(propertyProcessor, db, sqlGenerator)
        {
        }
        const string TableName = "supplierscoredetail";
        //public RSupplierScore(IMyDbFactory dbFactory)
        //{
        //    _dbFactory = dbFactory;
        //    _db = _dbFactory.Create("Mysql");
        //}
        public List<MSupplierScoreDetail> GetSupplierDeliveryRateDetail(int CompanyID, string SupplierName, MSupplierScoreParameter filter, string cond = "1")
        {
            List<MSupplierScoreDetail> result = new List<MSupplierScoreDetail>();
            cond = GetSqlCond(CompanyID);
            //透過公司ID取得供應商達交率明細
            string sql = $@"SELECT * FROM {TableName} WHERE {cond}";
            //執行SQL取得供應商達交率明細
            result = _db.GetListAsync<MSupplierScoreDetail>(sql, null).Result;
            return result;
        }
        private static string GetSqlCond(int CompanyID)
        {
            return $"company_fk={CompanyID}";
        }
        /// <summary>
        /// 取得供應商達交率
        /// </summary>
        /// <param name="pk">達交率明細ID</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public  Task<MSupplierScoreDetail?> GetByPk(int? pk)
        {
            string sql = $"SELECT * FROM {TableName} WHERE supplierId=@ID";
            var parameters = new DynamicParameters();
            parameters.Add("@ID", pk);
            return _db.GetListAsync<MSupplierScoreDetail>(sql, parameters).ContinueWith(t => t.Result.FirstOrDefault());
        }

        public  async Task<int> Insert(MSupplierScoreDetail obj, bool autoIncrement = true)
        {
            string cols = Utility.CmdColStr<MSupplierScoreDetail>(obj);
            string vals = Utility.CmdCValueStr<MSupplierScoreDetail>(obj);
            string sql = $@"INSERT INTO {TableName} ({cols}) VALUES({vals})";
            return await _db.ExecuteNonQueryAsync(sql, null);
        }

        public  Task<int> Update(MSupplierScoreDetail obj)
        {
            string cols = Utility.CmdColStr<MSupplierScoreDetail>(obj);
            string vals = Utility.CmdCValueStr<MSupplierScoreDetail>(obj);
            string sql = $"Update {TableName} set ";
            sql += $" `FactoryName` = @FactoryName,`TransactionNumber` = TransactionNumber,`SerialNumber` = @SerialNumber,`ItemNumber` = @ItemNumber,`ItemName` = @ItemName,`TransactionDate` = @TransactionDate,`EstimatedDeliveryDate` = @EstimatedDeliveryDate,`QuantityDelivered_Deadline` = @QuantityDelivered_Deadline,`DeadlineDeliveryQuantity` =@DeadlineDeliveryQuantity,`DeliveryRate` = @DeliveryRate,`PurchasingQuantity` =@PurchasingQuantity,`Factory_fk` = @Factory_fk,`company_fk` = @company_fk WHERE `id` = @pk";
            Dictionary<string, object?> dic = new Dictionary<string, object?>();
            dic.Add("@FactoryName", obj.FactoryName);
            dic.Add("@TransactionNumber", obj.TransactionNumber);
            dic.Add("@SerialNumber", obj.SerialNumber);
            dic.Add("@ItemNumber", obj.ItemNumber);
            dic.Add("@ItemName", obj.ItemName);
            dic.Add("@TransactionDate", obj.TransactionDate);
            dic.Add("@EstimatedDeliveryDate", obj.EstimatedDeliveryDate);
            dic.Add("@QuantityDelivered_Deadline", obj.QuantityDelivered_Deadline);
            dic.Add("@DeadlineDeliveryQuantity", obj.DeadlineDeliveryQuantity);
            dic.Add("@DeliveryRate", obj.DeliveryRate);
            dic.Add("@PurchasingQuantity", obj.PurchasingQuantity);
            dic.Add("@Factory_fk", obj.Factory_fk);
            dic.Add("@company_fk", obj.company_fk);
            dic.Add("@pk", obj.id);
            var parameters =Utility.CreateDynamicParameters(obj);

            return _db.ExecuteNonQueryAsync(sql, parameters);
        }

        public  async Task<int> Delete(int pk)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ID", pk);

            int affectedRows = await _db.ExecuteNonQueryAsync($"DELETE FROM {TableName} WHERE supplierId=@ID", parameters);
            return affectedRows;
        }

       

        public  Task<List<MSupplierScoreDetail>> GetAll()
        {
            // throw new NotImplementedException();
            string sql = $"SELECT * FROM {TableName}";
            return _db.GetListAsync<MSupplierScoreDetail>(sql, null);
        }
    }
}

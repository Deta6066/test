using Microsoft.AspNetCore.Http;
using VIS_API.Models;
using VIS_API.Models.WHE;


using VIS_API.Repositories.Interface;
using static VIS_API.Utilities.AllEnum;
using DapperDataBase.Database.Interface;
using VIS_API.Repositories.Base;
using VIS_API.Utilities;
using VIS_API.Service.Base;
using VIS_API.Models.PCD;
using VIS_API.SqlGenerator;
using Dapper;
using System.ComponentModel.Design;

namespace VIS_API.Repositories
{
    public class RInventory : GenericRepository<MInventoryDetail>, IRInventory
    {
        public RInventory(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<MInventoryDetail> sqlGenerator)
    : base(propertyProcessor, db, sqlGenerator)
        {
        }
        const string TABLENAME = "inventorydetail";
        public  Task<int> Delete(int pk)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 取得所有庫存明細
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public  async Task<List<MInventoryDetail>> GetAll()
        {
          
            string sql = $"SELECT * FROM `{TABLENAME}`";
            return await _db.GetListAsync<MInventoryDetail>(sql, null);
        }

        public  async Task<MInventoryDetail?> GetByPk(int? pk)
        {
            string sql = "";
            var parameters = new DynamicParameters();
            parameters.Add("@pk", pk);
            MInventoryDetail? detail = (await _db.GetListAsync<MInventoryDetail>(sql, parameters)).SingleOrDefault();
            return detail;
        }

        public  Task<int> Insert(MInventoryDetail obj, bool autoIncrement = true)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(MInventoryDetail obj)
        {
            throw new NotImplementedException();
        }
        public List<MInventoryDetail> GetInventoryDetail(MInventoryParameter filter, int companyID = 0)
        {
            List<MInventoryDetail> list = new List<MInventoryDetail>();
            string sql = $@"SELECT * FROM {TABLENAME} WHERE company_fk = {companyID}";
            //if (!string.IsNullOrEmpty(MaterialNo))
            //{
            //    sql += $" AND materialDescriptionNo = '{MaterialNo}'";
            //}
            sql+= GetSqlCond(filter);
            list =  _db.GetListAsync<MInventoryDetail>(sql, null).Result;
            return list;
        }
        /// <summary>
        /// 取得SQL條件
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string GetSqlCond(MInventoryParameter filter)
        {
            string cond = "";
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.Factory_fk))
                {
                    cond += $" AND Factory_fk = '{filter.Factory_fk}'";
                }
            }
            return cond;
        }

        public bool AddInventoryDetail(List<MInventoryDetail> modelList)
        {
            throw new NotImplementedException();
        }

        public Task<List<MInventoryDetail>> GetAll(string sql, Dictionary<string, object?> prams)
        {
            throw new NotImplementedException();
        }

        public Task<MInventoryDetail?> GetByPk(string? pk)
        {
            throw new NotImplementedException();
        }
    }
}

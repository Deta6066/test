using Dapper;
using DapperDataBase.Database.Interface;

using VIS_API.Models;
using VIS_API.Models.WHE;
using VIS_API.Repositories.Base;
using VIS_API.Repositories.Interface;
using VIS_API.Service.Base;
using VIS_API.SqlGenerator;
using VIS_API.Utilities;

namespace VIS_API.Repositories
{
    public class RStagnantMaterial: GenericRepository<MStagnantMaterial>, IRStagnantMaterial
    {
        public RStagnantMaterial(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<MStagnantMaterial> sqlGenerator)
    : base(propertyProcessor, db, sqlGenerator)
        {
        }
        const string TableName = "stagnantmaterial";
        public  Task<int> Delete(int pk)
        {
            throw new NotImplementedException();
        }

        public  Task<List<MStagnantMaterial>> GetAll()
        {
            string sql = $"SELECT * FROM {TableName}";
            return _db.GetListAsync<MStagnantMaterial>(sql, null);
        }

        

        public  Task<MStagnantMaterial?> GetByPk(int? pk)
        {
            throw new NotImplementedException();
        }

        public  List<MStagnantMaterial> GetStagnantMaterialList(int CompanyID, MStagnantMaterialParameter? filter)
        {
            List<MStagnantMaterial> list = new List<MStagnantMaterial>();
            string cond = GetSqlCond(CompanyID, filter);
            string sql = $@"SELECT * FROM {TableName} WHERE {cond}";
            
            var parameters = new DynamicParameters();
            parameters.Add("@company_fk", CompanyID);
            if (filter != null)
            {
                parameters.Add("@factory_fk", filter?.Factory_fk);
                parameters.Add("@warehouse", filter?.WarehouseLocation);
                parameters.Add("@MaterialCategory", filter?.MaterialCategory);
            }
            list = _db.GetListAsync<MStagnantMaterial>(sql, parameters).Result;
            return list;
        }
        /// <summary>
        /// 取得sql條件
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string GetSqlCond(int companyID, MStagnantMaterialParameter? filter)
        {
            string sql = $"company_fk=@company_fk";
            if (filter != null)
            {
                //廠區
                if (!string.IsNullOrEmpty(filter.Factory_fk))
                {
                    sql += " AND Factory_fk=@factory_fk";
                }
                //儲位
                if (filter.WarehouseLocation!= null)
                {
                    sql += " AND WarehouseLocation=@warehouse";
                }
                //物料類別
                if (filter.MaterialCategory.Count>0)
                {
                    // sql += " AND MaterialCategory=@MaterialCategory";
                    sql += " AND MaterialCategory in @MaterialCategory";
                }
            }
            return sql;
        }

        public Task<int> Insert(MStagnantMaterial obj, bool autoIncrement = true)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(MStagnantMaterial obj)
        {
            throw new NotImplementedException();
        }

       
    }
}

using Dapper;
using DapperDataBase.Database.Interface;
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
    public class RRole(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<MRole> sqlGenerator) : GenericRepositoryBase<MRole>(propertyProcessor, db, sqlGenerator), IRRole
    {
        public async Task<string?> GetAccess(uint pk)
        {
            string sql = @"
            SELECT `access`
            FROM `role`
            WHERE `pk` = @pk
            ;";
            var parameters = new DynamicParameters();
            parameters.Add("@pk", pk);
            return (await _db.GetListAsync<string>(sql, parameters)).SingleOrDefault();
        }

        public async Task<MRole?> GetByPk(int pk)
        {
            string sql = @"
            SELECT `pk`, `name`, `access`
            FROM `role`
            WHERE `pk` = @pk
            ;";

            var parameters = new DynamicParameters();
            parameters.Add("@pk", pk);
            return (await _db.GetListAsync<MRole>(sql, parameters)).SingleOrDefault();
        }

        public async Task<int> Insert(MRole obj, bool autoIncrement = true)
        {
            string sql = @"
            INSERT INTO `role`
            (`pk`, `name`, `access`)
            VALUES
            (@pk, @name, @access);

            SELECT LAST_INSERT_ID();
            ";
            var parameters = new DynamicParameters();
            parameters.Add("@pk", autoIncrement ? null : (object)obj.pk);
            parameters.Add("@name", obj.name);
            parameters.Add("@access", obj.access);
            return (await _db.ExecuteNonQueryAsync(sql, parameters)).Int();
        }

        public async Task<int> Update(MRole obj)
        {
            string sql = @"
            UPDATE `role`
            SET `name` = @name
            ,`access` = @access
            WHERE `pk` = @pk
            ;";
            var parameters = new DynamicParameters();
            parameters.Add("@name", obj.name);
            parameters.Add("@access", obj.access);
            parameters.Add("@pk", obj.pk);

            return await _db.ExecuteNonQueryAsync(sql, parameters);
        }

        public async Task<int> Delete(int pk)
        {
            string sql = @"
            DELETE FROM `role`
            WHERE `pk` = @pk
            ;";
            var parameters = new DynamicParameters();
            parameters.Add("@pk", pk);
            return await _db.ExecuteNonQueryAsync(sql, parameters);
        }

        
    }
}


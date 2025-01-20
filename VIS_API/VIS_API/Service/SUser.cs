using VIS_API.Utilities;

using VIS_API.Models;
using VIS_API.Service.Interface;
using DapperDataBase.Database.Interface;
using Dapper;
using VIS_API.Repositories.Interface;
using VIS_API.SqlGenerator;
using VIS_API.Repositories.Base;

namespace VIS_API.Service
{
    public class SUser : GenericRepositoryBase<MUser>, ISUser
    {
        #region 

        public async Task<MUser?> Get(uint pk)
        {
            string sql = @"
SELECT `department_fk`, `role_fk`, `pk`, `acc`, `name`, `pwd`, `email`, `phone`, `factory`, `color`, `status`, `created_at`, `updated_at`,`company_fk`
FROM `user`
WHERE `pk` = @pk
;";
            var parameters = new DynamicParameters();
            parameters.Add("@pk", pk);

            return (await _db.GetListAsync<MUser>(sql, parameters)).SingleOrDefault();

        }

        public async Task<uint> Insert(MUser obj, bool autoIncrement = true)
        {
            string sql = @"
INSERT INTO `user`
(`department_fk`, `role_fk`, `pk`, `acc`, `name`, `pwd`, `email`, `phone`, `factory`, `color`, `status`)
VALUES
(@department_fk, @role_fk, @pk, @acc, @name, @pwd, @email, @phone, @factory, @color, @status);

SELECT LAST_INSERT_ID();
";
            var parameters = new DynamicParameters();
            parameters.Add("@department_fk", obj.department_fk);
            parameters.Add("@role_fk", obj.role_fk);
            parameters.Add("@pk", autoIncrement ? null : (object)obj.pk);
            parameters.Add("@acc", obj.acc);
            parameters.Add("@name", obj.name);
            parameters.Add("@pwd", obj.pwd);
            parameters.Add("@email", obj.email);
            parameters.Add("@phone", obj.phone);
            parameters.Add("@factory", obj.factory);
            parameters.Add("@color", obj.color);
            parameters.Add("@status", obj.status);

            return (await _db.ExecuteScalarAsync<uint>(sql, parameters)).UInt();
        }

        public async Task<int> Update(MUser obj)
        {
            string sql = @"
UPDATE `user`
SET `department_fk` = @department_fk
,`role_fk` = @role_fk
,`acc` = @acc
,`name` = @name
,`pwd` = @pwd
,`email` = @email
,`phone` = @phone
,`factory` = @factory
,`color` = @color
,`status` = @status
WHERE `pk` = @pk
;";
            var parameters = new DynamicParameters();
            parameters.Add("@department_fk", obj.department_fk);
            parameters.Add("@role_fk", obj.role_fk);
            parameters.Add("@acc", obj.acc);
            parameters.Add("@name", obj.name);
            parameters.Add("@pwd", obj.pwd);
            parameters.Add("@email", obj.email);
            parameters.Add("@phone", obj.phone);
            parameters.Add("@factory", obj.factory);
            parameters.Add("@color", obj.color);
            parameters.Add("@status", obj.status);
            parameters.Add("@pk", obj.pk);

            return await _db.ExecuteNonQueryAsync(sql, parameters);
        }

        public async Task<int> Delete(uint pk)
        {
            string sql = @"
DELETE FROM `user`
WHERE `pk` = @pk
;";
            var parameters = new DynamicParameters();
            parameters.Add("@pk", pk);

            return await _db.ExecuteNonQueryAsync(sql, parameters);
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////

        
        public SUser(
            IPropertyProcessor propertyProcessor,
            IGenericDb db,
            ISqlGenerator<MUser> sqlGenerator
        ) : base(propertyProcessor, db, sqlGenerator)
        {
            // 如有需要，可在此進行額外初始化
        }
        public async Task<List<MUser>> GetList()
        {
            string sql = @"
SELECT `department_fk`, `role_fk`, `pk`, `acc`, `name`, `pwd`, `email`, `phone`, `factory`, `color`, `status`, `created_at`, `updated_at`,`company_fk`
FROM `user`
WHERE 1
;";
            return await _db.GetListAsync<MUser>(sql, new DynamicParameters());

        }

        public async Task<MUser?> GetByAcc(string acc,string pwd)
        {
            string sql = @"
                SELECT `department_fk`, `role_fk`, `pk`, `acc`, `name`, `pwd`, `email`, `phone`, `factory`, `color`, `status`, `created_at`, `updated_at`,`company_fk`
                FROM `user`
                WHERE `acc` = @acc and `pwd` = @pwd
                ;";
            var parameters = new DynamicParameters();
            parameters.Add("@acc", acc);
            parameters.Add("@pwd", pwd);
            return (await _db.GetListAsync<MUser>(sql, parameters)).SingleOrDefault();
        }
    }
}
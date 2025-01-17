using VisLibrary.Models;

using VisLibrary.Utilities;
using DapperDataBase.Database.Interface;
using Dapper;

namespace VisLibrary.Service
{
    public class Sdepartment
    {
        #region 

        public async Task<department?> Get(uint pk)
        {
            string sql = @"
            SELECT `pk`, `code`, `name`
            FROM `department`
            WHERE `pk` = @pk
            ;";
            var parameters = new DynamicParameters();
            parameters.Add("@pk", pk);
            
            return (await _db.GetListAsync<department>(sql, parameters)).SingleOrDefault();
        }

        public async Task<uint> Insert(department obj, bool autoIncrement = true)
        {
            string sql = @"
                INSERT INTO `department`
                (`pk`, `code`, `name`)
                VALUES
                (@pk, @code, @name);

                SELECT LAST_INSERT_ID();
                ";
            var parameters = new DynamicParameters();
            parameters.Add("@pk", autoIncrement ? null : obj.pk);
            parameters.Add("@code", obj.code);
            parameters.Add("@name", obj.name);

            return (await _db.ExecuteNonQueryAsync(sql, parameters)).UInt();
        }

        public async Task<int> Update(department obj)
        {
            string sql = @"
                UPDATE `department`
                SET `code` = @code
                ,`name` = @name
                WHERE `pk` = @pk
                ;";

            var parameters = new DynamicParameters();
            parameters.Add("@code", obj.code);
            parameters.Add("@name", obj.name);
            parameters.Add("@pk", obj.pk);

            return await _db.ExecuteNonQueryAsync(sql, parameters);
        }

        public async Task<int> Delete(uint pk)
        {
            string sql = @"
                DELETE FROM `department`
                WHERE `pk` = @pk
                ;";
            var parameters = new DynamicParameters();
            parameters.Add("@pk", pk);
            return await _db.ExecuteNonQueryAsync(sql, parameters);
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////

        private readonly IDb _db;

        public Sdepartment(IDb db)
        {
            _db = db;
        }

        public async Task<List<department>> GetList()
        {
            string sql = @"
SELECT `pk`, `code`, `name`
FROM `department`
WHERE 1
;";
            return await _db.GetListAsync<department>(sql, null);
        }


    }
}
using VisLibrary.Models;

using VisLibrary.Utilities;
using DapperDataBase.Database.Interface;
using Dapper;

namespace VisLibrary.Service
{
    public class SErrorLog
    {
        #region 

        public async Task<error_log?> Get(uint pk)
        {
            string sql = @"
                SELECT `pk`, `title`, `message`, `created_at`
                FROM `error_log`
                WHERE `pk` = @pk
                ;";
            var parameters = new DynamicParameters();
            parameters.Add("@pk", pk);
            
            return (await _db.GetListAsync<error_log>(sql, parameters)).SingleOrDefault();
        }

        public async Task<uint> Insert(error_log obj, bool autoIncrement = true)
        {
            string sql = @"
                INSERT INTO `error_log`
                (`pk`, `title`, `message`, `created_at`)
                VALUES
                (@pk, @title, @message, @created_at);

                SELECT LAST_INSERT_ID();
                ";

            var parameters = new DynamicParameters();
            parameters.Add("@pk", autoIncrement ? null : (object)obj.pk);
            parameters.Add("@title", obj.title);
            parameters.Add("@message", obj.message);
            parameters.Add("@created_at", obj.created_at);
            return (await _db.ExecuteNonQueryAsync(sql, parameters)).UInt();
        }

        public async Task<int> Update(error_log obj)
        {
            string sql = @"
            UPDATE `error_log`
            SET `title` = @title
            ,`message` = @message
            ,`created_at` = @created_at
            WHERE `pk` = @pk
            ;";
            var parameters = new DynamicParameters();
            parameters.Add("@title", obj.title);
            parameters.Add("@message", obj.message);
            parameters.Add("@created_at", obj.created_at);
            parameters.Add("@pk", obj.pk);

            return await _db.ExecuteNonQueryAsync(sql, parameters);
        }

        public async Task<int> Delete(uint pk)
        {
            string sql = @"
            DELETE FROM `error_log`
            WHERE `pk` = @pk
            ;";
            var parameters = new DynamicParameters();
            parameters.Add("@pk", pk);

            return await _db.ExecuteNonQueryAsync(sql, parameters);
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////

        private readonly IDb _db;

        public SErrorLog(IDb db)
        {
            _db = db;
        }

        public async Task<List<error_log>> GetList()
        {
            string sql = @"
SELECT `pk`, `title`, `message`, `created_at`
FROM `error_log`
WHERE 1
;";
            return await _db.GetListAsync<error_log>(sql, null);
        }


    }
}
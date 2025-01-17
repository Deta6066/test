using Dapper;
using DapperDataBase.Database.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisLibrary.Service.JWT
{
    public interface ISessionService
    {
        Task SetIPForAccountAsync(string account, string ip);
        Task<string?> GetIPByAccountAsync(string account);
        Task<int> DeleteSessionAsync(string account);
    }

    public class SessionServiceDapper : ISessionService
    {
        protected readonly IGenericDb _db;

        public SessionServiceDapper(IGenericDb db)
        {
            _db = db;
        }

        public async Task<int> DeleteSessionAsync(string account)
        {
            
            string sql = "DELETE FROM UserSession WHERE account = @account;";

            var parameters = new DynamicParameters();
            parameters.Add("@account", account);

            var rowsAffected = await _db.ExecuteNonQueryAsync(sql, parameters);
            return rowsAffected;
        }

        public async Task<string?> GetIPByAccountAsync(string account)
        {
            string sql = "SELECT ip FROM UserSession WHERE account = @account LIMIT 1;";
            var parameters = new DynamicParameters();
            parameters.Add("@account", account);

            var ip = await _db.ExecuteScalarAsync<string>(sql, parameters);
            return ip; // 可能為 null
        }

        public async Task SetIPForAccountAsync(string account, string ip)
        {
            // 檢查是否已有此帳號
            string selectSql = "SELECT account FROM UserSession WHERE account = @account LIMIT 1;";
            var parameters = new DynamicParameters();
            parameters.Add("@account", account);

            var existingAcc = await _db.ExecuteScalarAsync<string>(selectSql, parameters);

            if (string.IsNullOrEmpty(existingAcc))
            {
                // INSERT
                string insertSql = @"
                  INSERT INTO UserSession (account, ip, last_update)
                  VALUES (@account, @ip, @now);
                ";
                parameters.Add("@ip", ip);
                parameters.Add("@now", DateTime.Now);

                await _db.ExecuteNonQueryAsync(insertSql, parameters);
            }
            else
            {
                // UPDATE
                string updateSql = @"
                  UPDATE UserSession
                  SET ip = @ip,
                      last_update = @now
                  WHERE account = @account;
                ";
                parameters.Add("@ip", ip);
                parameters.Add("@now", DateTime.Now);

                await _db.ExecuteNonQueryAsync(updateSql, parameters);
            }
        }
    }

}

using Dapper;
using DapperDataBase.Database.Interface;
using Microsoft.AspNetCore.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models;
using VisLibrary.Repositories.Base;
using VisLibrary.Repositories.Interface;
using VisLibrary.SqlGenerator;

namespace VisLibrary.Repositories
{
    public class RefreshTokenRepository : GenericRepositoryBase<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<RefreshToken> sqlGenerator)
   : base(propertyProcessor, db, sqlGenerator)
        {
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
           
            string sql = "SELECT * FROM RefreshTokens WHERE Token = @Token";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Token", token);
            var result= await _db.GetListAsync<RefreshToken>(sql, parameters);
            return result.SingleOrDefault();
             
            
        }

        public async Task AddAsync(RefreshToken refreshToken)
        {
            string sql = @"
        INSERT INTO RefreshTokens (Token, Account, ExpiryDate, IsUsed, IsRevoked,CompanyFk)
        VALUES (@Token, @Account, @ExpiryDate, @IsUsed, @IsRevoked,@CompanyFk);
    ";
            DynamicParameters parameters = new DynamicParameters(refreshToken);
            await _db.ExecuteNonQueryAsync(sql, parameters);
        }

        public async Task UpdateAsync(RefreshToken refreshToken)
        {
            string sql = @"
                UPDATE RefreshTokens
                SET Token = @Token,
                    Account = @Account,
                    ExpiryDate = @ExpiryDate,
                    IsUsed = @IsUsed,
                    IsRevoked = @IsRevoked
                WHERE Id = @Id;
            ";
            DynamicParameters parameters = new DynamicParameters(refreshToken);
            await _db.ExecuteNonQueryAsync(sql, parameters);
        }
    }
}

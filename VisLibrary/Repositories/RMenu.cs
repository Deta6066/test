using VisLibrary.Models;

using VisLibrary.Repositories.Interface;
using VisLibrary.Utilities;
using DapperDataBase.Database.Interface;
using VisLibrary.Repositories.Base;
using VisLibrary.Service.Base;
using VisLibrary.SqlGenerator;
using Dapper;

namespace VisLibrary.Repositories
{
    public class RMenu(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<MMenu> sqlGenerator) : GenericRepository<MMenu>(propertyProcessor, db, sqlGenerator), IRMenu
    {
        #region 

        public async Task<List<MMenu>> GetAll()
        {
            List<MMenu>? result = new List<MMenu>();
           
            string sql = @"
                SELECT m.`pk`, m.`sort`, m.`node1`, m.`node2`, m.`icon`, m.`controller`, m.`extend`, m.`status`,d.`code` AS `code`
                FROM `menu` m
                JOIN `department` d ON m.`node1` = d.`pk`
                WHERE 1;";
                     result = await _db.GetListAsync<MMenu>(sql, null);
            return result;
        }

        public async Task<MMenu?> GetByPk(int? pk)
        {
            string sql = @"
                SELECT m.`pk`, m.`sort`, m.`node1`, m.`node2`, m.`icon`, m.`controller`, m.`extend`, m.`status`,d.`code` AS `code`
                FROM `menu` m
                JOIN `department` d ON m.`node1` = d.`pk`
                WHERE m.`pk` = @pk; ";
            var parameters = new DynamicParameters();
            parameters.Add("@pk", pk);
            return (await _db.GetListAsync<MMenu>(sql, parameters)).SingleOrDefault();
        }

        public async Task<int> Insert(MMenu obj, bool autoIncrement = true)
        {
            string sql = @"
                INSERT INTO `menu`
                (`pk`, `sort`, `node1`, `node2`, `icon`, `controller`, `extend`, `status`)
                VALUES
                (@pk, @sort, @node1, @node2, @icon, @controller, @extend, @status);

                SELECT LAST_INSERT_ID();
                ";
            var parameters = new DynamicParameters();
            parameters.Add("@pk", autoIncrement ? null : (object)obj.pk);
            parameters.Add("@sort", obj.sort);
            parameters.Add("@node1", obj.node1);
            parameters.Add("@node2", obj.node2);
            parameters.Add("@icon", obj.icon);
            parameters.Add("@controller", obj.controller);
            parameters.Add("@extend", obj.extend);
            parameters.Add("@status", obj.status);

            return (await _db.ExecuteNonQueryAsync(sql, parameters)).Int();
        }

        public async Task<int> Update(MMenu obj)
        {
            string sql = @"
                UPDATE `menu`
                SET `sort` = @sort
                ,`node1` = @node1
                ,`node2` = @node2
                ,`icon` = @icon
                ,`controller` = @controller
                ,`extend` = @extend
                ,`status` = @status
                WHERE `pk` = @pk
                ;";
            var parameters= Utility.CreateDynamicParameters(obj);
            return await _db.ExecuteNonQueryAsync(sql, parameters);

            //return await _db.ExecuteNonQueryAsync(sql, new Dictionary<string, object?>
            //{
            //    { "@sort", obj.sort },
            //    { "@node1", obj.node1 },
            //    { "@node2", obj.node2 },
            //    { "@icon", obj.icon },
            //    { "@controller", obj.controller },
            //    { "@extend", obj.extend },
            //    { "@status", obj.status },

            //    { "@pk", obj.pk },
            //});
        }

        public async Task<int> Delete(int pk)
        {
            string sql = @"
            DELETE FROM `menu`
            WHERE `pk` = @pk
            ;";
            var parameters = new DynamicParameters();
            parameters.Add("@pk", pk);
            return await _db.ExecuteNonQueryAsync(sql, parameters);
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////

    }
}
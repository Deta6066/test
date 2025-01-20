using DapperDataBase.Database.Interface;
using System.Collections.Concurrent;
using VIS_API.Models;
using VIS_API.Repositories.Interface;
using VIS_API.SqlGenerator;

namespace VIS_API.Repositories
{
    public class RCompany(IPropertyProcessor propertyProcessor, IGenericDb db, ISqlGenerator<MCompany> sqlGenerator) : GenericRepository<MCompany>(propertyProcessor, db, sqlGenerator)
    {
        public  Task<int> Delete(int pk)
        {
            throw new NotImplementedException();
        }

        public  Task<List<MCompany>> GetAll()
        {
            throw new NotImplementedException();
        }

        public  async Task<MCompany?> GetByPk(int? pk)
        {
            MCompany? company = null;
           
                    company = (await _db.GetListAsync<MCompany>($"SELECT * FROM company WHERE id = {pk}", null)).SingleOrDefault();
                  
                    //company = _db.Get<MCompany>($"SELECT * FROM company WHERE id = {pk}");
            
            return company;
        }

     

        public  Task<int> Insert(MCompany obj, bool autoIncrement = true)
        {
            throw new NotImplementedException();
        }

        public  Task<int> Update(MCompany obj)
        {
            throw new NotImplementedException();
        }
    }
}

using VisLibrary.Models;

namespace VisLibrary.Repositories.Interface
{
    public interface IRCompanyDataSource: IGenericRepositoryBase<MCompanyDataSource>
    {
        public  Task<MCompanyDataSource?> GetByPk(int? pk);
        public  Task<int> Insert(MCompanyDataSource obj, bool autoIncrement = true);
        public  Task<int> Update(MCompanyDataSource obj);
        public  Task<int> Delete(int pk);
        public  Task<List<MCompanyDataSource>> GetAll();
    }
}
